# Marketingreferral Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/MarketingReferral`

### API Endpoints
- **HTTPGET** `Invoices` (Method: `GetInvoices`)
- **HTTPGET** `BillDetails` (Method: `GetBillDetails`)
- **HTTPGET** `ReferralScheme` (Method: `GetReferralScheme`)
- **HTTPGET** `ReferringParty` (Method: `GetReferringParty`)
- **HTTPGET** `ReferringPartyGroup` (Method: `GetReferringPartyGroup`)
- **HTTPGET** `AlreadyAddedCommission` (Method: `GetAlreadyAddedCommission`)
- **HTTPGET** `ReferringOrganization` (Method: `GetReferringOrganizationList`)
- **HTTPGET** `MarketingreferralDetailReport` (Method: `GetMarketingreferralDetailReport`)
- **HTTPPOST** `NewReferralComission` (Method: `AddNewReferralComission`)
- **HTTPPOST** `NewReferringOrganization` (Method: `AddNewReferringOrganization`)
- **HTTPPOST** `NewReferringParty` (Method: `AddNewReferringParty`)
- **HTTPPUT** `ReferringOrganization` (Method: `UpdateReferringOrganization`)
- **HTTPPUT** `ReferringParty` (Method: `UpdateReferringParty`)
- **HTTPPUT** `ActivateDeactivateOrganization` (Method: `ActivateDeactivateOrganization`)
- **HTTPPUT** `ActivateDeactivateParty` (Method: `ActivateDeactivateParty`)
- **HTTPDELETE** `ReferralCommission` (Method: `DeleteReferralCommission`)

### Services
Backend services encapsulate business logic and data access for Marketingreferral.
- `IMarketingReferralService.cs`
- `MarketingReferralService.cs`

---

## Frontend Implementation
No dedicated frontend app folder found.
