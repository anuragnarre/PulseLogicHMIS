# Security Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Security`

### API Endpoints
- **HTTPGET** `LoggedInUserInformation` (Method: `GetLoggedInUserInformation`)
- **HTTPGET** `NavigationRoutes` (Method: `GetNavigationRoutelist`)
- **HTTPGET** `ValidRoutes` (Method: `GetAllValidRoutes`)
- **HTTPGET** `UserPermissions` (Method: `GetUserPermissions`)
- **HTTPGET** `ActiveBillingCounter` (Method: `GetactiveBillingCounter`)
- **HTTPGET** `ActiveLab` (Method: `GetActiveLab`)
- **HTTPGET** `ActiveInventory` (Method: `GetActiveInventory`)
- **HTTPGET** `ActivePharmacyCounter` (Method: `GetActivePharmacyCounter`)
- **HTTPGET** `ActiveAccountingHospitalInformation` (Method: `GetActiveAccHospitalInformation`)
- **HTTPGET** `InventeryHospitalInformation` (Method: `GetInventeryHospitalInformation`)
- **HTTPGET** `ActiveDispensary` (Method: `GetActiveDispencery`)
- **HTTPPUT** `ActivateBillingCounter` (Method: `PutActivateBillingCountery`)
- **HTTPPUT** `ActivatePharmacyCounter` (Method: `PutActivatePharmacyCounter`)
- **HTTPPUT** `ActivateDispensary` (Method: `PutActivateDispensary`)
- **HTTPPUT** `DeactivateDispensary` (Method: `PutDeactivateDispensary`)
- **HTTPPUT** `ActivateLab` (Method: `PutActivateLab`)
- **HTTPPUT** `DeactivateBillingCounter` (Method: `PutDeActivateBillingCounter`)
- **HTTPPUT** `ActivateInventory` (Method: `PutActivateInventory`)
- **HTTPPUT** `DeactivateInventory` (Method: `PutDeActivateInventory`)
- **HTTPPUT** `DeactivatePharmacyCounter` (Method: `PutDeActivatePharmacyCounter`)
- **HTTPPUT** `DeactivateLab` (Method: `PutDeactivateLab`)
- **HTTPPUT** `ActivateAccountingHospital` (Method: `PutActivateAccountingHospital`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/security`

### Angular Services & Endpoints
- `auth-guard.service.ts`
- `security.bl.service.ts`
- `security.dl.service.ts`
- `security.service.ts`

