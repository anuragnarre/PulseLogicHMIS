# Insurance Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Insurance`

### API Endpoints
- **HTTPGET** `GovInsurancePatients` (Method: `GovInsurancePatients`)
- **HTTPGET** `AdmittedPatients` (Method: `AdmittedPatients`)
- **HTTPGET** `AllRegisteredPatients` (Method: `AllRegisteredPatients`)
- **HTTPGET** `NewClaimCode` (Method: `NewClaimCode`)
- **HTTPGET** `OldClaimCode` (Method: `OldClaimCode`)
- **HTTPGET** `AdmissionOldClaimCode` (Method: `AdmissionOldClaimCode`)
- **HTTPGET** `DoctorNewOpdBillingItems` (Method: `DoctorNewOpdBillingItems`)
- **HTTPGET** `DepartmentNewOpdBillingItems` (Method: `DepartmentNewOpdBillingItems`)
- **HTTPGET** `DoctorFollowupBillingItems` (Method: `DoctorFollowupBillingItems`)
- **HTTPGET** `DepartmentFollowupBillingItems` (Method: `DepartmentFollowupBillingItems`)
- **HTTPGET** `DepartmentOldPatientBillingItems` (Method: `DepartmentOldPatientBillingItems`)
- **HTTPGET** `DoctorOldPatientBillingItems` (Method: `DoctorOldPatientBillingItems`)
- **HTTPGET** `Doctors` (Method: `Doctors`)
- **HTTPGET** `BillCfgItems` (Method: `BillCfgItems`)
- **HTTPGET** `AppointmentApplicableDepartments` (Method: `AppointemntApplicableDepartments`)
- **HTTPGET** `PatientHealthCardWithBillInfo` (Method: `PatientHealthCardWithBillInfo`)
- **HTTPGET** `OpdTicketInvoiceInfo` (Method: `OpdTicketInvoiceInfo`)
- **HTTPGET** `MatchingPatients` (Method: `MatchingPatients`)
- **HTTPGET** `PatientsByNshiNumber` (Method: `PatientsByNshiNumber`)
- **HTTPGET** `InsuranceProviders` (Method: `InsuranceProviders`)
- **HTTPGET** `PatientBillingContext` (Method: `PatientBillingContext`)
- **HTTPGET** `CreditOrganizations` (Method: `CreditOrganizations`)
- **HTTPGET** `IsClaimCodeValid` (Method: `IsClaimCodeValid`)
- **HTTPGET** `TodaysPatientVisit` (Method: `TodaysPatientVisit`)
- **HTTPGET** `PatientsVisits` (Method: `PatientsVisits`)
- **HTTPGET** `VisitInfoForStickerPrint` (Method: `VisitInfoForStickerPrint`)
- **HTTPGET** `InsuranceApplicableBillCfgItems` (Method: `InsuranceApplicableBillCfgItems`)
- **HTTPGET** `LatestClaimCode` (Method: `LatestClaimCode`)
- **HTTPGET** `InPatientDetailForPartialBilling` (Method: `InPatientDetailForPartialBilling`)
- **HTTPGET** `AppointmentApplicableDoctors` (Method: `DoctorLists`)
- **HTTPGET** `ActiveEmployees` (Method: `ActiveEmployees`)
- **HTTPGET** `PatientCurrentVisitContext` (Method: `PatientCurrentVisitContext`)
- **HTTPGET** `PerformerWisePatientVisits` (Method: `PerformerWisePatientVisits`)
- **HTTPGET** `PatientDeposits` (Method: `PatientDeposits`)
- **HTTPGET** `PatientPastBillSummary` (Method: `PatientPastBillSummary`)
- **HTTPGET** `AppointmentApplicableEmployees` (Method: `AppointmentApplicableEmployees`)
- **HTTPGET** `PatientPendingItems` (Method: `PatientPendingItems`)
- **HTTPGET** `CheckCreditBill` (Method: `CheckCreditBill`)
- **HTTPGET** `DischargeReceipt` (Method: `DischargeReceipt`)
- **HTTPGET** `PatientBalanceHitstory` (Method: `PatientBalanceHitstory`)
- **HTTPGET** `PatientClaimCodes` (Method: `PatientClaimCodes`)
- **HTTPGET** `PatientClaimDetail` (Method: `PatientClaimDetail`)
- **HTTPGET** `PatientsByClaimCode` (Method: `PatientsByClaimCode`)
- **HTTPPOST** `PostInsuranceBilling` (Method: `PostInsuranceBilling`)
- **HTTPPOST** `PostInsuranceProvisionalBilling` (Method: `PostInsuranceProvisionalBilling`)
- **HTTPPOST** `NewPatient` (Method: `GovInsurancePatient`)
- **HTTPPOST** `CreateVisit` (Method: `CreateVisit`)
- **HTTPPOST** `SaveHTMLFile` (Method: `SaveHTMLFile`)
- **HTTPPOST** `FreeFollowup` (Method: `FreeFollowup`)
- **HTTPPOST** `PaidFollowup` (Method: `PaidFollowup`)
- **HTTPPOST** `BillingTransactionItems` (Method: `BillingTransactionItems`)
- **HTTPPOST** `CreateBillingVisit` (Method: `BillingVisits`)
- **HTTPPOST** `Lab/Requisition` (Method: `NewRequisitions`)
- **HTTPPOST** `Radiology/Requisition` (Method: `RequestItems`)
- **HTTPPOST** `PatientDeposit` (Method: `PatientDeposit`)
- **HTTPPOST** `Discharge` (Method: `Discharge`)
- **HTTPPOST** `BillingTransaction` (Method: `BillingTransaction`)
- **HTTPPUT** `Patient` (Method: `Patient`)
- **HTTPPUT** `Procedure` (Method: `Procedure`)
- **HTTPPUT** `EditItemPriceQtyDiscAndProvider` (Method: `EditItemPriceQtyDiscAndProvider`)
- **HTTPPUT** `Discharge` (Method: `DischargeFromBilling`)
- **HTTPPUT** `BillingTransactionItems` (Method: `BillTxnItems`)
- **HTTPPUT** `Balance` (Method: `Balance`)
- **HTTPGET** `MedicareMemberDetail` (Method: `GetMedicareMemberDetail`)
- **HTTPGET** `Departments` (Method: `Departments`)
- **HTTPGET** `Designations` (Method: `Designations`)
- **HTTPGET** `MedicareTypes` (Method: `MedicareTypes`)
- **HTTPGET** `MedicareInstitutes` (Method: `MedicareInstitutes`)
- **HTTPGET** `InsuranceProviders` (Method: `InsuranceProviders`)
- **HTTPGET** `MedicareMemberByPatientId` (Method: `MedicareMemberDetailByPatientId`)
- **HTTPGET** `MedicareMemberByMemberNo` (Method: `MedicareMemberDetailByMedicareNo`)
- **HTTPGET** `DependentMedicareMember` (Method: `DependentMedicareMember`)
- **HTTPGET** `MedicarePatientList` (Method: `MedicarePatientList`)
- **HTTPPOST** `MedicareMemberDetails` (Method: `AddMedicareMemberDetails`)
- **HTTPPUT** `MedicareMemberDetails` (Method: `UpdateMedicareMemberDetails`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/insurance`

### Routing / Views
- `/Patient`
- `/Visit`
- `/IPDBilling`
- `/Reports`
- `/InsNewVisit`
- `/BillingRequest`
- `/**`
- `/GovInsTotalItemsBill`
- `/GovInsIncomeSegregation`
- `/GovInsPatientWiseClaims`

### Angular Components
- `medicare-registration-main`
- `medicare-dependent`
- `medicare-member`
- `gov-insurance-main`
- `gov-ins-billing-request`
- `gov-ins-ipd-billing-patient-list`
- `gov-ins-edit-bill-item`
- `gov-insurance-ip-patient-summary`
- `gov-ins-update-item-price`
- `gov-ins-ip-billing-request-slip`
- `gov-ins-patient-list`
- `gov-ins-update-balance`
- `gov-ins-patient-registration`
- `gov-ins-price-history`
- `gov-ins-reports-main`
- `gov-ins-income-segregation`
- `gov-ins-patient-wise-claims`
- `gov-ins-total-items-bill`
- `ins-visit-list`
- `ins-followup-visit`
- `ins-new-visit-billing-info`
- `ins-new-visit-info`
- `ins-new-visit-main`
- `ins-new-visit-patient-info`
- `ins-patient-duplicate-warning-box`
- `ins-pat-claim-details-view`
- `ins-sticker-print`

### Angular Services & Endpoints
- `medicare.bl.service.ts`
- `medicare.dl.service.ts`
- `medicare.service.ts`
- `insurance.bl.service.ts`
- `insurance.dl.service.ts`
- `ssf.service.ts`

