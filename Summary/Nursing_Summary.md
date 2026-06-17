# Nursing Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Nursing`

### API Endpoints
- **HTTPGET** `OpdVisits` (Method: `OpdVisits`)
- **HTTPGET** `PastVisits` (Method: `PastVisits`)
- **HTTPGET** `Complains` (Method: `Complains`)
- **HTTPGET** `getNephrologyPatients` (Method: `getNephrologyPatients`)
- **HTTPPOST** `FavouritePatient` (Method: `FavouritePatient`)
- **HTTPPOST** `ClinicalInformation` (Method: `ClinicalInformationlains`)
- **HTTPPOST** `Complaint` (Method: `complaint`)
- **HTTPPOST** `CheckInDetails` (Method: `CheckInDetails`)
- **HTTPPOST** `VisitForFreeReferral` (Method: `VisitForFreeReferral`)
- **HTTPPOST** `CheckOutDetails` (Method: `CheckOutDetails`)
- **HTTPPUT** `ExchangeDoctorDepartment` (Method: `ExchangeDoctorDepartment`)
- **HTTPPUT** `ClinicalInformation` (Method: `ClinicalInformation`)
- **HTTPPUT** `Complaint` (Method: `Complaint`)
- **HTTPPUT** `RemoveFromPreference` (Method: `RemoveFromPreference`)
- **HTTPGET** `GetAllDepartments` (Method: `GetAllDepartments`)
- **HTTPGET** `GetBillingDetails` (Method: `GetBillingDetails`)
- **HTTPGET** `InvestigationResults` (Method: `InvestigationResults`)

### Services
Backend services encapsulate business logic and data access for Nursing.

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/nursing`

### Routing / Views
- `/OutPatient`
- `/InPatient`
- `/InPatientList`
- `/ActivateWard`
- `/**`
- `/PatientOverviewMain`
- `/PatientOverview`
- `/InvestigationResults`
- `/Clinical`
- `/WardBilling`
- `/ScannedImages`
- `/DrugsRequest`
- `/Transfer`
- `/ConsultationRequests`
- `/Notes`
- `/Nephrology`
- `/RequisitionList`
- `/DischargeSummary`

### Angular Components
- `nursing-main`
- `nursing-opd-checkin`
- `nursing-opd-chekout`
- `consultation-requests`
- `consultation-request-view-print`
- `new-request`
- `nephrology`
- `nursing-inpatient`
- `nursing-outpatient`
- `nursing-receive-note`
- `transferred-patient-pending`
- `activate-ward`
- `add-patient-diet`
- `diet-sheet`
- `diet-sheet-patient-history`
- `diet-sheet-print`
- `drug-request-list`
- `drugs-request`
- `exchange-doctor-department`
- `nursing-opd-free-referral`
- `investigation-results`
- `investigation-results-print`
- `discharge-summary-list`
- `nursing-transfer`
- `opd-triage`
- `nursing-order-list`
- `nursing-order-main`
- `nursing-order`
- `nursing-add-diagnosis`
- `nursing-ip-billitem-request`
- `nursing-ip-request`
- `nursing-ward-billing`

### Angular Services & Endpoints
- `nursing.bl.service.ts`
- `nursing.dl.service.ts`
- `ward-selection-guard.service.ts`

