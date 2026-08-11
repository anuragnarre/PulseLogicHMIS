# PulseLogicHMR — Working of Key Fields & Data Models

> **Last Updated:** 2026-07-14 | **Version:** v2.0.1
> **Change Note:** Documenting the core data models, their relationships, and how key fields are processed in the system following v2.0.1 security fixes.

---

## 1. Security & Authentication Fields

### `RbacUser`
The core entity representing a system user.

| Field | Description | v2.0.1 Notes |
|-------|-------------|--------------|
| `UserName` | Unique identifier (string) | Used for login and auditing. |
| `Password` | Encrypted password (string) | **Crucial Fix:** v2.0.1 fixed an auth bypass where the input password was compared to itself instead of the hashed DB password. Currently uses TripleDES/ECB (Tech Debt: PLHMR-SEC-001 to migrate to BCrypt). |
| `IsActive` | Boolean | Disables account access if false. |
| `Roles` | `List<RbacRole>` | Aggregates all roles assigned to the user. |

### `RbacPermission`
Defines granular access rights.

| Field | Description | Working Mechanism |
|-------|-------------|-------------------|
| `PermissionName` | string identifier | e.g., `"Billing-View"`. The `[PulseLogicViewFilter("PermissionName")]` attribute on controllers checks this string against the user's cached `validpermissionlist` in the session. |

### JWT Token Payload
Generated upon successful authentication.
- **Issuer/Audience:** `pulselogic-emr-prod` (Enforced in v2.0.1).
- **Claims:** Contains `nameid` (UserName), roles, and department context.
- **Expiration:** 480 minutes (8 hours) in production.

---

## 2. Global Search Component Fields

The `global-search.component.ts` operates in the Angular topbar.

| Field / State | Working Mechanism | v2.0.1 Notes |
|---------------|-------------------|--------------|
| `searchTerm` | Bound to the input via `[(ngModel)]`. Requires >= 2 chars to trigger. | Triggers debounced API call. |
| `results` | Array of `SearchResult` objects. | Contains `title`, `subtitle`, `type` (e.g., Patient, Setting), and `icon`. |
| `isLoading` | Boolean | Toggles the spinner icon. |
| Error Handling | Subscription `err` callback | **Fix:** v2.0.1 added `msgBox.showMessage('error', ...)` to display UI alerts instead of silent console failures. |

---

## 3. Patient Context Fields

When a patient is selected, the application operates in a "Patient Context".

| Field | Description | Display / Working |
|-------|-------------|-------------------|
| `PatientCode` | Hospital Number (HN) | Primary key for patient tracking. When not null, the UI replaces the global search with a "Patient Context Chip". |
| `ShortName` | Patient name | Displayed in the topbar. |
| `DateOfBirth` / `Gender` | Demographics | Filtered through the `PulseLogicDateTime:'age'` pipe to display format like "34Y / M". |
| `MembershipTypeId` | Discount category | Evaluated against DB parameter `"MembershipTypeDiscount"`. If active, displays a star badge. |

---

## 4. File Upload Fields (Multipart/form-data)

Processed by `FileUploader.Upload()`.

| Field / Object | Description | v2.0.1 Security Enforcement |
|----------------|-------------|-----------------------------|
| `files` | `IFormFileCollection` | The uploaded payload. |
| `file.Length` | Size in bytes | **Fix:** Now strictly checked against `MaxFileSizeBytes` (50 MB). |
| `file.FileName` | Original client filename | **Fix:** Passed through `Path.GetFileName()` to strip any malicious path traversal sequences (e.g., `../../../malicious.exe`). |
| `extension` | Extracted from filename | **Fix:** Checked against an explicit `HashSet` whitelist (`.pdf`, `.dcm`, `.jpg`, etc.). |
| `localFolder` | Target sub-directory | **Fix:** Bound via `Path.GetFullPath()` check to ensure it cannot escape the `fileStorageLocation` root. |
| `responseData` | `PulseLogicHTTPResponse` | **Fix:** If an exception occurs, `ErrorMessage` is set to a generic string. Server stack traces and internal paths (`ex.Message`) are NEVER mapped to this field anymore. |

---

## 5. System Parameters (`CoreDbContext.Parameters`)

Configuration flags stored in the database.

| Field | Description | Working Mechanism |
|-------|-------------|-------------------|
| `ParameterGroupName` | e.g., `"Billing"` | Logical grouping. |
| `ParameterName` | e.g., `"EnableTax"` | The specific flag. |
| `ParameterValue` | string representation | Read and converted by `CommonFunctions.cs`. |

**v2.0.1 Fix (B3):** When a `ParameterName` is missing from the DB, `FirstOrDefault()` returns null. The system previously crashed trying to access `.ParameterValue`. It now uses `param?.ParameterValue` and returns safe defaults (`0`, `false`, `""`).
