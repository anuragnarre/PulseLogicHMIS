# Claimmanagement Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/ClaimManagement`

### API Endpoints
- **HTTPGET** `InsuranceApplicableCreditOrganizations` (Method: `InsuranceApplicableCreditOrganizations`)
- **HTTPGET** `BillReview` (Method: `GetBillForReview`)
- **HTTPGET** `IsClaimCodeAvailable` (Method: `IsClaimCodeAvailable`)
- **HTTPGET** `PendingClaims` (Method: `GetPendingClaims`)
- **HTTPGET** `Claim/Invoices` (Method: `GetInvoicesByClaimId`)
- **HTTPGET** `Claim/PreviewDocument` (Method: `GetDocumentForPreviewByFileId`)
- **HTTPGET** `Claim/Documents` (Method: `GetDocumentsByClaimId`)
- **HTTPGET** `PaymentPendingClaims` (Method: `GetPaymentPendingClaims`)
- **HTTPGET** `InsurancePayments` (Method: `GetInsurancePayments`)
- **HTTPGET** `ClaimDetails` (Method: `ClaimDetailsForPreview`)
- **HTTPGET** `BillingCreditNotes` (Method: `BillingCreditNotes`)
- **HTTPGET** `PharmacyCreditNotes` (Method: `PharmacyCreditNotes`)
- **HTTPGET** `BillingCreditBillItems` (Method: `BillingCreditBillItems`)
- **HTTPGET** `PharmacyCreditBillItems` (Method: `PharmacyCreditBillItems`)
- **HTTPGET** `ApiIntegrationNameByOrganizationId` (Method: `ApiIntegrationNameByOrganizationId`)
- **HTTPGET** `ECHSPatientWithVisitInformation` (Method: `ECHSPatientWithVisitInformation`)
- **HTTPGET** `DashboardAnalytics` (Method: `GetDashboardAnalyticsAsync`)
- **HTTPPOST** `InsuranceClaim` (Method: `SendClaimForScrubbing`)
- **HTTPPOST** `SubmitClaim` (Method: `SubmitClaim`)
- **HTTPPOST** `InsuranceClaimPayment` (Method: `InsuranceClaimPayment`)
- **HTTPPUT** `ClaimableStatus` (Method: `ChangeClaimableStatus`)
- **HTTPPUT** `Claim/Invoice/ClaimableStatus` (Method: `UpdateClaimableStatusOfClaimedInvoice`)
- **HTTPPUT** `RevertToBillReview` (Method: `RevertInvoiceBackToBillReview`)
- **HTTPPUT** `SaveClaimAsDraft` (Method: `SaveClaimAsDraft`)
- **HTTPPUT** `ClaimCode` (Method: `UpdateClaimCode`)
- **HTTPPUT** `ClaimApprovedAndRejectedAmount` (Method: `UpdateApprovedAndRejectedAmount`)
- **HTTPPUT** `ConcludeClaim` (Method: `ConcludeClaim`)
- **HTTPPUT** `RevertToClaimScrubbing` (Method: `RevertClaimToBackToClaimScrubbing`)
- **HTTPPUT** `BillingCreditItemClaimableStatus` (Method: `BillingCreditItemClaimableStatus`)
- **HTTPPUT** `PharmacyCreditItemClaimableStatus` (Method: `PharmacyCreditItemClaimableStatus`)
- **HTTPPUT** `InsuranceClaimPayment` (Method: `UpdateInsuranceClaimPayment`)

### Services
Backend services encapsulate business logic and data access for Claimmanagement.
- `ClaimManagementService.cs`
- `IClaimManagementService.cs`
- `UploadedFileDTO.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/claim-management`

### Routing / Views
- `/SelectInsuranceProvider`
- `/BillReview`
- `/Scrubbing`
- `/PaymentProcessing`
- `/Reports`
- `/ClaimForms`
- `/FormSelection`
- `/EchsMrpDrugCertificate`
- `/MedicalClaim`
- `/SSFClaim`
- `/**`

### Angular Components
- `claim-management-main`
- `ins-bill-assign-claim-code`
- `ins-bill-list`
- `ins-bill-preview`
- `claim-forms`
- `echs-mrp-drug-certificate`
- `form-selection`
- `medical-claim-form`
- `new-payment`
- `payment`
- `view-payment`
- `reports`
- `ins-claim-preview`
- `ins-claim-scrubbing`
- `ins-claims-list`
- `ins-provider-selection`
- `document-upload`
- `echs-mrp-drug-certificate-print`
- `medical-claim-form-print`
- `ssf-claim`
- `tpa-dashboard`

### Angular Services & Endpoints
- `claim-management.bl.service.ts`
- `claim-management.dl.service.ts`
- `claim-management.service.ts`

