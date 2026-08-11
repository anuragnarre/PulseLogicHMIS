# PulseLogicHMR — System Working & Functional Overview

> **Last Updated:** 2026-07-14 | **Version:** v2.0.1
> **Change Note:** Auth bypass fixed (S5), rate limiting added (S6), session hardened (S7), CSRF protection added (S9), DICOM auth fixed (S18), exception handling improved.

---

## Application Bootstrap Flow

```
Browser Request
    │
    ├─► IIS / Kestrel receives request
    │
    ├─► Program.cs → CreateHostBuilder → Startup.cs
    │       ├─ ConfigureServices()   ← Dependency injection, JWT, RBAC, Rate Limiter
    │       └─ Configure()           ← Middleware pipeline
    │
    ├─► Middleware Pipeline (in order):
    │       1. ExceptionHandlingMiddleware      ← Global error catch (wraps all below)
    │       2. SecurityHeadersMiddleware        ← ← ADDED v2.0.1: X-Frame-Options, CSP etc.
    │       3. UseHsts()                        ← HTTPS Strict Transport Security
    │       4. UseHttpsRedirection()            ← HTTP → HTTPS redirect
    │       5. UseResponseCompression()         ← ← ADDED v2.0.1: gzip/brotli
    │       6. UseStaticFiles()                 ← wwwroot serving
    │       7. UseRouting()
    │       8. UseAuthentication()              ← JWT Bearer validation
    │       9. UseAuthorization()
    │      10. UseSession()                     ← Server-side session (RBAC cache)
    │      11. UseRateLimiter()                 ← ← ADDED v2.0.1: login 10/min, API 300/min
    │      12. UseMvc()                         ← MVC routing + action filters
    │
    └─► Response returned
```

---

## Authentication & Session Flow

### 1. Login
```
POST /Account/Login
    │
    ├─► Rate limit check: 10 requests/min per IP  ← NEW v2.0.1
    ├─► ModelState validation
    ├─► RBAC.GetUser(username, EncryptPassword(password))
    │       └─ TripleDES/ECB hash comparison
    │          ⚠️ TECH DEBT: Migrate to BCrypt (tracked: PLHMR-SEC-001)
    ├─► If valid:
    │       ├─ Load RbacUser → store in Session["currentuser"]
    │       ├─ Load permissions → store in Session["validpermissionlist"]
    │       ├─ Generate JWT token (signed with ASPNETCORE_JWT_KEY)
    │       ├─ Set Remember-Me cookie (HttpOnly, Secure, SameSite=Strict)  ← FIXED v2.0.1
    │       └─ Redirect to home
    └─► If invalid:
            └─ Return error view (no stack trace exposed)
```

### 2. Per-Request RBAC Check (MVC Views)
```
Request to any [PulseLogicViewFilter] decorated action
    │
    ├─► PulseLogicViewFilter.OnActionExecuting()
    │       ├─ Read Session["currentuser"]
    │       ├─ If NULL → Redirect to /Account/Login   ← FIXED v2.0.1 (was: silent pass-through)
    │       ├─ Read Session["validpermissionlist"]
    │       ├─ Find matching permission for PermissionName
    │       └─ If not found → Redirect to /Account/PageNotFound
    └─► Action executes
```

### 3. DICOM API Endpoint Auth
```
POST /api/Dicom (from listener.exe)
    │
    ├─► PulseLogicActionFilter (API filter)
    ├─► Reads "currentuser" from request body JSON
    ├─► RBAC.GetUser(userName, password)   ← FIXED v2.0.1 (was: IsValidUser() — always true)
    └─► If null → return { Status: "Failed", ErrorMessage: "Unauthorized Access" }
```

### 4. Logout
```
POST /Account/Logout   ← CHANGED v2.0.1 (was GET — CSRF-vulnerable)
    │
    ├─► [ValidateAntiForgeryToken] validates CSRF token
    ├─► Session.Clear()
    ├─► Clear all auth cookies
    └─► Redirect to /Account/Login
```

Angular side:
```typescript
// app.component.ts — LogoutFromAplication()
// Creates a hidden form, injects CSRF token from DOM, submits via POST
// ← FIXED v2.0.1: was window.location.href = '/Account/Logout' (GET)
```

---

## JWT Token Lifecycle

| Step | Detail |
|------|--------|
| **Generation** | On successful login, signed with `ASPNETCORE_JWT_KEY` (env var) or appsettings fallback |
| **Secret Source** | Env var `ASPNETCORE_JWT_KEY` → appsettings `JwtTokenConfig.secret` (with yellow startup warning) |
| **Lifetime** | Dev: configured minutes. Production: 480 min (8h) |
| **Validation** | `ValidateIssuer = true`, `ValidateAudience = true`, `ValidateLifetime = true` — all enabled ← FIXED v2.0.1 |
| **HTTPS** | `RequireHttpsMetadata = true` in production ← FIXED v2.0.1 |
| **Storage** | Angular stores in `localStorage` under `ENUM_LocalStorageKeys.LoginTokenName` |
| **Expiry behaviour** | Angular clears localStorage + session on token expiry, redirects to login |

---

## Rate Limiting (v2.0.1)

| Policy | Window | Limit | Applied To |
|--------|--------|-------|------------|
| `"login"` | 60 seconds (fixed) | 10 requests per IP | `POST /Account/Login` |
| `"api-general"` | 60 seconds (sliding) | 300 requests per IP | Ready for `[EnableRateLimiting("api-general")]` on controllers |
| Rejection response | — | HTTP 429 Too Many Requests | All rate-limited endpoints |

---

## Session Management

| Property | Value | Notes |
|----------|-------|-------|
| Provider | SQL Server Distributed Cache | Configured in `ConfigureServices.cs` |
| Timeout | 2 hours idle | `options.IdleTimeout = TimeSpan.FromHours(2)` |
| Cookie HttpOnly | `true` | Prevents JS access |
| Cookie Secure | `SameAsRequest` | HTTPS-only in production ← FIXED v2.0.1 (was `.Secure = true` — compile error) |
| Cookie SameSite | `Strict` | Prevents cross-site requests ← ADDED v2.0.1 |

---

## File Upload Workflow

```
User selects file(s) in Angular component
    │
    ├─► Angular POST to /api/[Module]/Upload (multipart/form-data)
    ├─► C# Controller receives IFormFileCollection
    ├─► FileUploader.Upload(files, localFolder)
    │       ├─ Size check: > 50 MB → reject        ← ADDED v2.0.1
    │       ├─ Filename: Path.GetFileName()         ← FIXED v2.0.1 (path traversal)
    │       ├─ Extension: whitelist check           ← ADDED v2.0.1
    │       ├─ Path: GetFullPath() boundary guard   ← ADDED v2.0.1
    │       └─ Write to disk
    └─► Return { Status: "OK", Results: "relative/path" }
        OR  { Status: "Failed", ErrorMessage: "safe message" }   ← FIXED v2.0.1
```

---

## Global Exception Handling

All unhandled exceptions are caught by `ExceptionHandlingMiddleware` (registered first in pipeline):

| Exception Type | HTTP Status | Client Message | Log Level |
|----------------|-------------|----------------|-----------|
| `OperationCanceledException` (client abort) | 499 | — (no body) | Information ← NEW |
| `UnauthorizedAccessException` | 401 | "Access denied." | Warning ← NEW |
| `KeyNotFoundException` | 404 | Exception message (safe) | Warning |
| `ArgumentException` | 400 | Exception message (safe) | Warning |
| `InvalidOperationException` | 422 | Exception message (safe) | Warning |
| `Exception` (all others) | 500 | "An unexpected server error occurred." | Error |

> **Security:** Stack traces and internal paths are **never** returned to the client. Full details logged server-side with `context.TraceIdentifier` as correlation ID.

---

## RBAC Permission Model

```
RbacUser
  └─ has many RbacRole
       └─ has many RbacPermission
            └─ PermissionName (string, matched per-action)

Session["currentuser"]         → RbacUser (serialized JSON)
Session["validpermissionlist"] → List<RbacPermission>
```

Permissions are loaded on login and cached in session for the duration. Cache is invalidated on session expiry or manual logout.

---

## System Parameters (`Parameters` table)

Runtime configuration loaded from DB via:
```csharp
// CommonFunctions.cs
GetCoreParameterIntValue(ctx, "GroupName", "ParamName")
GetCoreParameterStringValue(ctx, "GroupName", "ParamName")
GetCoreParameterBoolValue(ctx, "GroupName", "ParamName")
```

> **Bug Fix v2.0.1 (B3):** All three methods now use null-conditional operator (`?.ParameterValue`) to avoid `NullReferenceException` when a parameter name does not exist in the database. Previously, any missing parameter caused an unhandled exception.

---

## Change Log (Functional)

| Date | Change | Impact |
|------|--------|--------|
| 2026-07-14 | Fixed `IsValidUser()` auth bypass (S5) | Critical: all users now properly authenticated |
| 2026-07-14 | Fixed DICOM endpoint auth (S18) | Critical: listener.exe now properly authenticated |
| 2026-07-14 | Fixed null session pass-through (B2) | Session-expired users now redirected to login |
| 2026-07-14 | Fixed `GetCoreParameter*` null ref (B3) | Missing DB params no longer crash the server |
| 2026-07-14 | Logout changed to POST + CSRF (S9) | Prevents forced logout via image/link attacks |
| 2026-07-14 | JWT validation enabled (S4) | Tokens from other issuers now rejected |
| 2026-07-14 | Rate limiting added (S6) | Brute force protection on login |
| 2026-07-14 | File upload whitelist + path guard (S12/S13) | Prevents malicious file uploads and path traversal |
| 2026-07-14 | Exception details hidden from clients (S8) | Server paths/stack traces no longer leaked |
