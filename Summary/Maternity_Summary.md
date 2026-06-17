# Maternity Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Maternity`

### API Endpoints
- **HTTPPOST** `AddMaternityPatient` (Method: `AddMaternityPatient`)
- **HTTPGET** `GetAllDosesNumber` (Method: `GetAllDosesNumber`)
- **HTTPGET** `GetDatForEditSearch` (Method: `GetDatForEditSearch`)
- **HTTPGET** `GetPatientDetails` (Method: `GetPatientDetails`)
- **HTTPGET** `GetPatientDetailById` (Method: `GetPatientDetailById`)
- **HTTPGET** `GetAllActiveMaternityPatientList` (Method: `GetAllActiveMaternityPatientList`)
- **HTTPGET** `GetAllANCByMaternityPatId` (Method: `GetAllANCByMaternityPatId`)
- **HTTPGET** `GetAllFilesUploadedbyMaternityPatId` (Method: `GetAllFilesUploadedbyMaternityPatId`)
- **HTTPGET** `GetAllBabyDetailsByMaternityPatId` (Method: `GetAllBabyDetailsByMaternityPatId`)
- **HTTPGET** `DownloadFile` (Method: `Download`)
- **HTTPGET** `SearchPatListForAllowance` (Method: `SearchPatListForAllowance`)
- **HTTPGET** `GetMaternityAllowanceReportList` (Method: `GetMaternityAllowanceReportList`)
- **HTTPPOST** `UpdateMaternityPatient` (Method: `UpdateMaternityPatient`)
- **HTTPPOST** `AddUpdateMaternityANC` (Method: `AddUpdateMaternityANC`)
- **HTTPPOST** `RegisterMaternity` (Method: `RegisterMaternity`)
- **HTTPPOST** `UpdateChildInfo` (Method: `UpdateChildInfo`)
- **HTTPPOST** `UpdateMotherInfo` (Method: `UpdateMotherInfo`)
- **HTTPPOST** `UploadMaternityPatientFiles` (Method: `UploadMaternityPatientFiles`)
- **HTTPDELETE** `DeleteMaternityPatient` (Method: `DeleteMaternityPatient`)
- **HTTPDELETE** `DeleteMaternityPatientANC` (Method: `DeleteMaternityPatientANC`)
- **HTTPDELETE** `DeleteMaternityPatientFile` (Method: `DeleteMaternityPatientFile`)
- **HTTPDELETE** `ConcludeMaternityPatient` (Method: `ConcludeMaternityPatient`)
- **HTTPDELETE** `RemoveChild` (Method: `RemoveChild`)
- **HTTPGET** `GetPatientPaymentDetailById` (Method: `GetPatientPaymentDetailById`)
- **HTTPGET** `GetPatientPaymentDetailByPaymentId` (Method: `GetPatientPaymentDetailByPaymentId`)
- **HTTPPOST** `AddMaternityPatientPayment` (Method: `AddMaternityPatientPayment`)

### Services
Backend services encapsulate business logic and data access for Maternity.
- `IMaternityService.cs`
- `MaternityService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/maternity`

### Routing / Views
- `/PatientList`
- `/Payments`
- `/PaymentPatientList`
- `/PaymentDetails`
- `/**`
- `/Reports`
- `/MaternityAllowance`

### Angular Components
- `maternity-main`
- `maternity-patient-list`
- `maternity-anc`
- `maternity-file-upload`
- `maternity-patient-add`
- `maternity-register`
- `maternity-payments.main`
- `maternity-patient-payment`
- `mat-payment-patient-list`
- `maternity-payment-receipt`
- `maternity-reports`
- `mat-allowance-report`

### Angular Services & Endpoints
- `maternity.bl.service.ts`
- `maternity.dl.service.ts`
- `maternity.service.ts`

