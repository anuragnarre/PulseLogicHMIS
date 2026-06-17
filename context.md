# PulseLogicHMR v2.0 — Project Context

> **Last updated:** 2026-06-07  
> **Reason for this session:** Crash caused login-page hang. Full clean rebuild performed: backend, frontend (Angular), and both databases.

---

## 1. What This Project Is

**PulseLogicHMR** is a full-stack Hospital Management & EMR (Electronic Medical Record) system. It is a fork/rebranded version of PulseLogicHMR, customised heavily for international hospital deployments.

- **Target:** Hospital staff (doctors, nurses, admins, billing, pharmacy, lab, radiology, etc.)
- **Deployment:** Self-hosted, Windows Server / IIS or local `dotnet run`
- **Version:** v2.0 (assembly version 4.6.1.10)

---

## 2. Technology Stack

### Backend — ASP.NET Core (.NET 8)
| Item | Detail |
|---|---|
| Framework | ASP.NET Core MVC + Web API, .NET 8 (`net8.0`) |
| ORM | Entity Framework Core 8 + classic EF 6 (dual) |
| Auth | Session-based RBAC + JWT Bearer (dual) |
| Password crypto | TripleDES/ECB with MD5-derived key, salt = `PulseLogicsalt` |
| Caching | `PulseLogicCache` singleton (in-memory, configurable expiry) |
| Audit | Audit.NET → SQL Server (`PulseLogicAudit` table, togglable via `IsAuditEnable`) |
| DI | Custom `AddPulseLogicServices()` extension in `ConfigureServices.cs` |
| Serialisation | Newtonsoft.Json (`DefaultContractResolver`) |
| API docs | Swagger/OpenAPI v1 (dev mode only) |
| Local .NET SDK | `d:\...\PulseLogicHMR_v2.0\.dotnet\dotnet.exe` (bundled .NET 8) |
| Run script | `RunApp.ps1` → sets PATH to local dotnet, then `dotnet run` |

### Frontend — Angular 7
| Item | Detail |
|---|---|
| Framework | Angular 7.x |
| Build tool | Angular CLI 7.3.6 |
| UI libs | Angular Material 7, Bootstrap 4, ag-Grid 20, amCharts 3, ngx-webcam, CKEditor |
| Charts | amCharts 3, Chart.js 2 |
| Reporting | jsPDF, iTextSharp (server), html2canvas, xlsx |
| Medical | CornerstoneJS (DICOM viewer), QR/barcode, PDF-lib |
| Build output | `wwwroot/PulseLogicApp/dist/` |
| Node modules | `wwwroot/PulseLogicApp/node_modules/` |
| Entry point served | `wwwroot/UI/Main.html` (set in Startup.cs DefaultFiles) |

### Databases — SQL Server Express
| Database | Purpose |
|---|---|
| `DEV_PulseLogicHMR_INT` | Main EMR database (patients, billing, lab, pharmacy, etc.) |
| `PulseLogicAdmin` | Admin/RBAC database (users, roles, permissions, routes) |
| Server | `localhost\SQLEXPRESS` (Integrated Security) |
| Connection strings | In `appsettings.json` — Integrated Security (no password in dev) |

---

## 3. Solution / Project Structure

```
PulseLogicHMR_v2.0/
├── .dotnet/                        ← Bundled .NET 8 SDK (use for dotnet commands)
├── RunApp.ps1                      ← Start the app (sets PATH + dotnet run)
├── Database/
│   ├── 1. Admin-Db/
│   │   └── 1. PulseLogicAdmin_CompleteDB.sql   ← Full Admin DB creation script
│   ├── 2. EMR-Db/
│   │   └── PulseLogicInternationalDB/
│   │       └── extracted/Dev_PulseLogicHMR_INT1.bak ← EMR DB backup (132MB)
│   └── CleanUpScript.sql           ← Wipes all transaction data (keeps config)
└── Code/
    ├── Components/                 ← Class library projects (DLLs)
    │   ├── PulseLogicHMR.Core/     ← Caching, Configuration, NepaliDate, RBAC engine
    │   ├── PulseLogicHMR.DalLayer/ ← EF DbContexts (MasterDbContext, etc.)
    │   ├── PulseLogicHMR.Security/ ← RBAC class, password encryption/decryption
    │   ├── PulseLogicHMR.ServerModel/ ← Entity models, ViewModels
    │   ├── PulseLogicHMR.AccTransfer/ ← Accounting transfer logic
    │   ├── PulseLogicHMR.Sync/     ← Real-time sync (RealTimeRemoteSyncEnabled)
    │   ├── PulseLogicHMR.Jobs/     ← Background jobs
    │   └── PulseLogicHMR.Tests/    ← Unit tests
    └── Websites/
        └── PulseLogicHMR/          ← Main ASP.NET Core web host
            ├── Startup.cs          ← App wiring (services, middleware, routing)
            ├── ConfigureServices.cs← DI extension for all services
            ├── Program.cs
            ├── appsettings.json    ← Connection strings, JWT config, flags
            ├── Controllers/        ← 47+ module controllers
            └── wwwroot/
                └── PulseLogicApp/  ← Angular SPA
                    ├── src/app/    ← 50+ Angular feature modules
                    ├── dist/       ← Build output (served by ASP.NET)
                    └── package.json
```

---

## 4. Key Configuration — `appsettings.json`

```json
{
  "Connectionstring":      "...DEV_PulseLogicHMR_INT...",
  "ConnectionStringAdmin": "...PulseLogicAdmin...",
  "environment": { "isdevelopment": true },
  "CacheExpirationMinutes": 0,
  "ngbuildpath": "wwwroot\\PulseLogicApp",
  "IsAuditEnable": false,
  "RealTimeRemoteSyncEnabled": false,
  "ApplicationVersionNum": "v2.0.0",
  "JwtTokenConfig": { "JwtValidMinutes": 1440 }
}
```

---

## 5. Module Map (Controllers ↔ Angular Modules)

| Domain | Backend Controller Dir | Angular Module Dir |
|---|---|---|
| Security/Login | `Security/SecurityController.cs` | `security/` |
| Patient | `Patient/` | `patients/` |
| Billing | `Billing/` | `billing/` |
| Pharmacy | `Pharmacy/` | `pharmacy/` |
| Dispensary | `Dispensary/` | `dispensary/` |
| Lab | `Lab/` | `labs/` |
| Radiology | `Radiology/` | `radiology/` |
| Admission (ADT) | `Admission/` | `adt/` |
| Clinical | `Clinical/` | `clinical/` |
| Accounting | `Accounting/` | `accounting/` |
| Inventory | `Inventory/` | `inventory/` |
| Emergency | `Emergency/` | `emergency/` |
| Nursing | `Nursing/` | `nursing/` |
| OT | `OperationTheatre/` | `ot-module/` |
| Payroll | `Payroll/` | `payroll-module/` |
| Maternity | `Maternity/` | `maternity/` |
| Ward Supply | `WardSupply/` | `wardsupply/` |
| Employee | `Employee/` | `employee/` |
| Scheduling | `Scheduling/` | `scheduling/` |
| Incentive | `Incentive/` | `incentive/` |
| Insurance | `Insurance/` | `insurance/` |
| Settings | `Settings/` | `settings-new/` |
| System Admin | `SystemAdmin/` | `system-admin/` |
| Reporting | `Reporting/` | `reporting/` |
| Dashboard | `Dashboard/` | `dashboards/` |
| CSSD | `CSSD/` | `cssd/` |
| Vaccination | `Vaccination/` | `vaccination/` |
| Fixed Asset | `FixedAsset/` | `fixed-asset/` |

---

## 6. Auth / Login Flow

1. User POSTs credentials to `AccountController` → `Login` action
2. Server validates against `PulseLogicAdmin` DB using RBAC
3. Password compared using **TripleDES-ECB** decrypt (salt: `PulseLogicsalt`)
4. On success: `RbacUser` object stored in **ASP.NET Session** (`"currentuser"`)
5. JWT token also issued for stateless API calls
6. Angular `app.component.ts` calls `SecurityController` → `NavigationRoutes`, `ValidRoutes`, `UserPermissions` to load sidebar and permissions
7. Session timeout: **2 hours** (IdleTimeout in Startup.cs)

> ⚠️ **Known Issue (this session):** After a server crash, ASP.NET in-memory sessions are wiped. Angular loads the login page successfully but after login the `NavigationRoutes` call returns `"Session Expired"` because the session is gone. This caused the app to appear "stuck" on login. **Resolution: Full rebuild was performed instead of debugging.**

---

## 7. How to Build & Run

### Run the App
```powershell
# From repo root:
.\RunApp.ps1
# This sets PATH to the bundled .dotnet and runs: dotnet run
```

### Build Backend Only
```powershell
$env:PATH = "d:\App and Hardware Project\App\PulseLogicHMR_v2.0\.dotnet;" + $env:PATH
cd "d:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR"
dotnet build
```

### Build Angular Frontend
```cmd
:: Use cmd.exe (npm.ps1 blocked by PS execution policy)
cd "d:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp"
npm install --legacy-peer-deps
npm run build
```
> ⚠️ Always use `--legacy-peer-deps` with npm install — Angular 7 packages have peer dependency conflicts with newer npm strict resolution.

### Rebuild Databases
```powershell
# Admin DB (SQL script):
sqlcmd -S "localhost\SQLEXPRESS" -i "Database\1. Admin-Db\1. PulseLogicAdmin_CompleteDB.sql"

# EMR DB (from .bak):
sqlcmd -S "localhost\SQLEXPRESS" -i "Database\restore_emr.sql"
# (restore_emr.sql uses MOVE to place files in MSSQL16.SQLEXPRESS\MSSQL\DATA\)
```

---

## 8. Known Gotchas

| # | Issue | Cause | Fix |
|---|---|---|---|
| 1 | App stuck after login (session expired) | Server restart wipes in-memory ASP.NET session | Rebuild / restart server freshly |
| 2 | `npm install` fails with ERESOLVE | Angular 7 peer dep mismatch in newer npm | Always use `--legacy-peer-deps` |
| 3 | `dotnet clean/build` fails with NETSDK1047 | System dotnet (2.1.818) picks up instead of bundled 8.x | Set PATH to `.dotnet` folder before any dotnet command, OR use `RunApp.ps1` |
| 4 | `npm` blocked in PowerShell | npm.ps1 execution policy | Use `cmd.exe /c "npm ..."` instead |
| 5 | EMR DB FILESTREAM restore | FILESTREAM group in .bak needs a folder + MOVE | Use `restore_emr.sql` script (created this session) |
| 6 | Password encryption | TripleDES-ECB, MD5 key from salt `PulseLogicsalt` | See `test_encrypt.cs` / `test_decrypt.cs` in repo root |

---

## 9. Files Created This Session

| File | Purpose |
|---|---|
| `Database/restore_emr.sql` | Restores `DEV_PulseLogicHMR_INT` from `.bak` to MSSQL16 data dir |
| `context.md` (this file) | Project context for future AI conversations |

---

## 10. Rebuild Status (2026-06-07)

| Step | Status |
|---|---|
| `dotnet clean` | ✅ Done |
| `dotnet build` (backend) | ✅ Done — 0 errors, 8786 warnings (CA2200 only) |
| `PulseLogicAdmin` DB restore | ✅ Done (from SQL script) |
| `DEV_PulseLogicHMR_INT` DB restore | ✅ Done (16,098 pages from .bak in 73s) |
| `npm install --legacy-peer-deps` | ✅ Done |
| `npm run build` (Angular) | ✅ Done |
