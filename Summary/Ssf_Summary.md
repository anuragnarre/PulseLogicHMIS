# Ssf Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/SSF`

### API Endpoints
- **HTTPGET** `GetSSFPatientData` (Method: `GetSSFPatientData`)
- **HTTPPOST** `CheckSSFEligibility` (Method: `CheckSSFEligibility`)
- **HTTPGET** `GetEmployerList` (Method: `GetEmployerList`)
- **HTTPPOST** `SubmitClaim` (Method: `SubmitClaim`)
- **HTTPPOST** `BookClaim` (Method: `BookClaim`)
- **HTTPGET** `GetClaimBookingDetail` (Method: `GetClaimBookingDetail`)
- **HTTPGET** `GetClaimDetail` (Method: `GetClaimDetail`)
- **HTTPGET** `CheckClaimStatusLocally` (Method: `IsClaimed`)
- **HTTPGET** `GetSSFPatientDetailLocally` (Method: `GetSSFPatientDetailLocally`)

### Services
Backend services encapsulate business logic and data access for Ssf.
- `ISSFService.cs`
- `SSFService.cs`

---

## Frontend Implementation
No dedicated frontend app folder found.
