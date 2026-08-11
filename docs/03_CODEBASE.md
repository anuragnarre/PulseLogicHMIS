# PulseLogicHMR — Codebase Reference

> **Last Updated:** 2026-07-14 | **Version:** v2.0.1
> **Change Note:** Security hardening across RBAC, ActionFilter, FileUploader, CommonFunctions, AccountController, Startup, ConfigureServices. New files: SecurityHeadersMiddleware.cs. Deleted: phantom wwwroot dirs.

---

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET / ASP.NET Core | 8.0.421 |
| Language (Backend) | C# | 12 |
| Frontend Framework | Angular | 7 |
| Frontend Language | TypeScript | ~3.x |
| ORM | Entity Framework Core | 8.x |
| Auth | JWT Bearer + Session-based RBAC | Custom |
| Caching | SQL Server Distributed Cache | — |
| Compression | Microsoft.AspNetCore.ResponseCompression | Built-in |
| Rate Limiting | Microsoft.AspNetCore.RateLimiting | Built-in (.NET 8) |
| DICOM Viewer | CornerstoneJS + WADO Image Loader | — |
| Device Detection | Wangkanai.Detection | — |
| AutoMapper | AutoMapper | — |

---

## Solution Structure

```
PulseLogicHMR_v2.0.1/
├── Code/
│   ├── Components/
│   │   ├── PulseLogicHMR.CommonTypes/      ← Shared DTOs, enums, response wrappers
│   │   └── PulseLogicHMR.Security/         ← RBAC engine
│   │       ├── RBAC/
│   │       │   └── PulseLogicRBAC.cs       ← Auth + user management  ← MODIFIED v2.0.1
│   │       └── Models/
│   │           ├── RbacUser.cs
│   │           ├── RbacRole.cs
│   │           └── RbacPermission.cs
│   ├── Solutions/                          ← Solution files
│   └── Websites/
│       └── PulseLogicHMR/                  ← Main ASP.NET Core web application
│           ├── Controllers/
│           │   ├── AccountController.cs    ← Login/Logout/Auth  ← MODIFIED v2.0.1
│           │   ├── HomeController.cs
│           │   ├── PulseLogicActionFilter.cs  ← RBAC + DICOM auth filter  ← MODIFIED v2.0.1
│           │   ├── [Module]/               ← 49 module controller dirs
│           │   └── ...
│           ├── Services/                   ← Business logic (49 service dirs)
│           ├── Utilities/
│           │   ├── CommonFunctions.cs      ← Param helpers  ← MODIFIED v2.0.1
│           │   ├── ExceptionHandlingMiddleware.cs  ← Global error handler  ← MODIFIED v2.0.1
│           │   ├── FileUploader.cs         ← File upload handler  ← REWRITTEN v2.0.1
│           │   ├── SecurityHeadersMiddleware.cs    ← ← NEW v2.0.1
│           │   ├── ImageUploader.cs
│           │   └── ...
│           ├── ViewModel/                  ← View model DTOs
│           ├── Views/                      ← Razor views (.cshtml)
│           │   └── Account/
│           │       └── Login.cshtml        ← Login UI (3-theme, glassmorphism)
│           ├── DependencyInjection/        ← Extension methods for service registration
│           ├── ConfigureServices.cs        ← JWT + Service config  ← MODIFIED v2.0.1
│           ├── Startup.cs                  ← App pipeline  ← MODIFIED v2.0.1
│           ├── Program.cs
│           ├── appsettings.json            ← Dev config (excluded from VCS)
│           ├── appsettings.Production.json ← Prod config  ← POPULATED v2.0.1
│           └── wwwroot/
│               └── PulseLogicApp/          ← Angular SPA
│                   └── src/app/
│                       ├── app.component.ts   ← Root component  ← MODIFIED v2.0.1
│                       ├── app.module.ts
│                       └── [51 feature dirs]
├── Database/                               ← SQL scripts
├── docs/                                   ← ← NEW v2.0.1: this documentation folder
└── RunApp.ps1                              ← Launch script
```

---

## Key Files — Quick Reference

### Backend

| File | Role | v2.0.1 Changes |
|------|------|----------------|
| [PulseLogicRBAC.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Components/PulseLogicHMR.Security/RBAC/PulseLogicRBAC.cs) | User auth, permission loading, password hashing | Fixed `IsValidUser()` self-comparison bug (S5); documented TripleDES tech debt |
| [ConfigureServices.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/ConfigureServices.cs) | JWT + service DI configuration | JWT key from env var (S1); issuer/audience validation (S4); HTTPS enforcement (S11) |
| [Startup.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Startup.cs) | Middleware pipeline, session, Swagger | Session cookie SameSite+SecurePolicy (S7); Rate limiter (S6); Swagger gated to dev (S10); SecurityHeaders registered |
| [AccountController.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Controllers/AccountController.cs) | Login, logout, cookie management | Logout → HttpPost+CSRF (S9); Remember-me cookie HttpOnly+Secure+SameSite (S16); Rate limit on login |
| [PulseLogicActionFilter.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Controllers/PulseLogicActionFilter.cs) | Per-request RBAC check, DICOM API auth | B2: null session → redirect to Login; S18: DICOM uses GetUser() not IsValidUser() |
| [FileUploader.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Utilities/FileUploader.cs) | Handles multipart file uploads to disk | **Fully rewritten**: path traversal guard, filename sanitisation, type whitelist, size limit, no exception leakage |
| [CommonFunctions.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Utilities/CommonFunctions.cs) | DB parameter helpers, common utilities | B3: null-safe `?.ParameterValue` in all GetCoreParameter* methods |
| [ExceptionHandlingMiddleware.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Utilities/ExceptionHandlingMiddleware.cs) | Global exception handler | Added UnauthorizedAccessException (401) and OperationCanceledException (499, info-level) handlers |
| [SecurityHeadersMiddleware.cs](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/Utilities/SecurityHeadersMiddleware.cs) | HTTP security headers | **New file** — injects X-Frame-Options, X-Content-Type-Options, Referrer-Policy, Permissions-Policy, Cache-Control |

### Frontend (Angular)

| File | Role | v2.0.1 Changes |
|------|------|----------------|
| [app.component.ts](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/app.component.ts) | Root component, session init, nav loading | Replaced 5× `alert()` with `msgBoxServ.showMessage()`; `logError` → `console.error`; `LogoutFromAplication` → form POST (CSRF compatible) |
| [global-search.component.ts](file:///d:/App%20and%20Hardware%20Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/shared/global-search/global-search.component.ts) | Global spotlight search | Error handler now surfaces user-visible message via msgBox |

---

## Configuration Files

### `appsettings.json` (development — excluded from VCS)
```jsonc
{
  "environment": { "isdevelopment": true },
  "JwtTokenConfig": {
    "secret": "...",          // fallback only — use env var in prod
    "JwtAudience": "...",
    "JwtIssuer": "...",
    "JwtValidMinutes": 1440
  },
  "ConnectionStrings": { ... }
}
```

### `appsettings.Production.json` (populated v2.0.1)
```jsonc
{
  "environment": { "isdevelopment": false },
  "IsAuditEnable": true,
  "CacheExpirationMinutes": 60,
  "JwtTokenConfig": {
    "JwtValidMinutes": 480,
    "JwtAudience": "pulselogic-emr-prod",
    "JwtIssuer": "pulselogic-emr-prod"
  },
  "Logging": { "LogLevel": { "Default": "Warning" } }
}
```

---

## Naming Conventions

| Area | Convention | Example |
|------|-----------|---------|
| Controllers | PascalCase + `Controller` suffix | `AccountController.cs` |
| Services | PascalCase + `Service` suffix | `PatientService.cs` |
| ViewModels | PascalCase + `ViewModel` suffix | `LoginViewModel.cs` |
| Angular components | kebab-case | `global-search.component.ts` |
| Angular services | camelCase + `Service` suffix | `coreService` |
| DB parameters | `GroupName` / `ParameterName` pairs | `"Billing"` / `"MembershipTypeDiscount"` |

---

## NuGet Dependencies (Key)

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Microsoft.AspNetCore.RateLimiting" />          <!-- .NET 8 built-in -->
<PackageReference Include="Microsoft.AspNetCore.ResponseCompression" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="Microsoft.Extensions.Caching.SqlServer" />     <!-- Session store -->
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" />
<PackageReference Include="Wangkanai.Detection" />                         <!-- Device detection -->
<PackageReference Include="Newtonsoft.Json" />                             <!-- PulseLogicJSONConvert wrapper -->
```

---

## Outstanding Tech Debt

| ID | Area | Description | Priority |
|----|------|-------------|----------|
| PLHMR-SEC-001 | Password hashing | Migrate from TripleDES/ECB to BCrypt. Lazy re-hash on login (3-step plan in PulseLogicRBAC.cs comments) | High |
| PLHMR-SEC-012 | File serving | Move `fileuploads/` outside `wwwroot`; serve via authenticated endpoint | High |
| PLHMR-PERF-001 | Rate limiting | Apply `[EnableRateLimiting("api-general")]` to all API base controllers | Medium |
| PLHMR-WARN-001 | Exception re-throw | `throw ex` in many service catch blocks loses stack trace — change to `throw` | Low |

---

## Build Commands

```powershell
# Build backend
& "d:\App and Hardware Project\App\PulseLogicHMR_v2.0.1\.dotnet\dotnet.exe" build .\PulseLogicHMR.csproj

# Build Angular (from wwwroot/PulseLogicApp)
npm install --legacy-peer-deps
npx ng build --prod

# Run in development
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_JWT_KEY = "<your-256-bit-base64-key>"
& "..\.dotnet\dotnet.exe" run
```

---

## Change Log (Codebase)

| Date | File | Change |
|------|------|--------|
| 2026-07-14 | `PulseLogicRBAC.cs` | Fixed auth bypass — `IsValidUser` `a.Password == a.Password` → proper hash compare |
| 2026-07-14 | `ConfigureServices.cs` | JWT key from env var; enabled issuer/audience/HTTPS validation |
| 2026-07-14 | `Startup.cs` | Session SecurePolicy; rate limiter; response compression; security headers; Swagger gated |
| 2026-07-14 | `AccountController.cs` | Logout → POST+CSRF; remember-me cookie hardened; rate limit attribute |
| 2026-07-14 | `PulseLogicActionFilter.cs` | Null session → redirect to Login; DICOM uses `GetUser()` |
| 2026-07-14 | `FileUploader.cs` | Full rewrite: path guard, type whitelist, size limit, safe errors |
| 2026-07-14 | `CommonFunctions.cs` | Null-safe parameter lookups (B3) |
| 2026-07-14 | `ExceptionHandlingMiddleware.cs` | Added 401 and 499 handlers |
| 2026-07-14 | `SecurityHeadersMiddleware.cs` | **New**: security response headers |
| 2026-07-14 | `appsettings.Production.json` | Populated from empty `{}` |
| 2026-07-14 | `app.component.ts` | `alert()` → `msgBoxServ`; logout → form POST |
| 2026-07-14 | `global-search.component.ts` | Error UX via msgBox |
| 2026-07-14 | `.gitignore` | Hardened (PHI uploads, logs, dist, IDE files) |
