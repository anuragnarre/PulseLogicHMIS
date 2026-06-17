# Cssd Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/CSSD`

### API Endpoints
- **HTTPGET** `GetIntegratedCssdReport` (Method: `GetIntegratedCssdReport`)
- **HTTPGET** `GetAllPendingCSSDTransactions` (Method: `GetAllPendingCSSDTransactions`)
- **HTTPGET** `GetAllFinalizedCSSDTransactions` (Method: `GetAllFinalizedCSSDTransactions`)
- **HTTPPUT** `DisinfectCSSDItem` (Method: `DisinfectCSSDItem`)
- **HTTPPUT** `DispatchCSSDItem` (Method: `DispatchCSSDItem`)

### Services
Backend services encapsulate business logic and data access for Cssd.
- `CssdItemService.cs`
- `CssdReportService.cs`
- `ICssdItemService.cs`
- `ICssdReportService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/cssd`

### Routing / Views
- `/Sterilization`
- `/PendingItems`
- `/FinalizedItems`
- `/**`
- `/Reports`
- `/IntegratedCSSD`

### Angular Components
- `cssd-main`
- `reports`
- `integrated-cssd-report`
- `sterilization`
- `sterilization-finalized-items`
- `sterilization-pending-items`
- `disinfect-item`

### Angular Services & Endpoints
- `cssd-report.endpoint.ts`
- `sterilization.endpoint.ts`
- `sterilization.service.ts`

