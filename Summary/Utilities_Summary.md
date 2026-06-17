# Utilities Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Utilities`

### API Endpoints
- **HTTPGET** `SchemeRefund` (Method: `GetSchemeRefund`)
- **HTTPGET** `SchemeRefundById` (Method: `GetSchemeRefundById`)
- **HTTPGET** `PatientSchemeRefunds` (Method: `GetPatientSchemeRefunds`)
- **HTTPPOST** `SchemeRefund` (Method: `SaveSchemeRefund`)
- **HTTPPOST** `ChangeVisitScheme` (Method: `SaveChangedVisitScheme`)
- **HTTPPOST** `OrganizationDeposit` (Method: `OrganizationDeposit`)
- **HTTPGET** `OrganizationDepositBalance` (Method: `GetOrganizationDepositBalance`)
- **HTTPGET** `OrganizationDepositDetailById` (Method: `GetOrganizationDepositDetails`)

### Services
Backend services encapsulate business logic and data access for Utilities.
- `ICacheService.cs`
- `IUtilitiesService.cs`
- `MemoryCacheService.cs`
- `UtilitiesService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/utilities`

### Routing / Views
- `/SchemeRefund`
- `/ChangeVisitScheme`
- `/ChangeBillingCounter`
- `/OrganizationDeposit`
- `/**`

### Angular Components
- `utilities-main`
- `change-billing-counter`
- `change-scheme-price-category`
- `organization-deposit`
- `print-organization-deposit`
- `scheme-refund-print`
- `scheme-refund-list`
- `scheme-refund`
- `process-confirmation`

### Angular Services & Endpoints
- `activate-billing-counter.service.ts`
- `utilities.bl.service.ts`
- `utilities.dl.service.ts`
- `utilities.service.ts`

