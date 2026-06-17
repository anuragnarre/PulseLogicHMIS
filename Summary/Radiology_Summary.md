# Radiology Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Radiology`

### API Endpoints
- **HTTPGET** `FilmTypes` (Method: `GetFilmTypes`)
- **HTTPGET** `Requisitions` (Method: `GetRequisitions`)
- **HTTPGET** `ImagingReports` (Method: `GetImagingReports`)
- **HTTPGET** `ImagingReport` (Method: `GetImagingReport`)
- **HTTPGET** `ImagingResults` (Method: `ImagingResults`)
- **HTTPGET** `PatientVisitsImagingResults` (Method: `PatientVisitsImagingResults`)
- **HTTPGET** `ImagingItems` (Method: `ImagingItems`)
- **HTTPGET** `ReportDetail` (Method: `ReportDetail`)
- **HTTPGET** `ImgingFilesFromPACS` (Method: `ImgingFilesFromPACS`)
- **HTTPGET** `ReportTemplates` (Method: `ReportTemplates`)
- **HTTPGET** `DicomImage` (Method: `DicomImage`)
- **HTTPGET** `DicomImages` (Method: `DicomImages`)
- **HTTPGET** `Doctors` (Method: `Doctors`)
- **HTTPGET** `ImagingTypes` (Method: `ImagingTypes`)
- **HTTPPOST** `Requisitions` (Method: `Requisitions`)
- **HTTPPOST** `Report` (Method: `Report`)
- **HTTPPOST** `PatientStudy` (Method: `PostPatientStudy`)
- **HTTPPOST** `SendEmail` (Method: `SendEmail`)
- **HTTPPUT** `ImagingReport` (Method: `ImagingReport`)
- **HTTPPUT** `BillingStatus` (Method: `BillingStatus`)
- **HTTPPUT** `DeleteReportImages` (Method: `DeleteReportImages`)
- **HTTPPUT** `PatientStudy` (Method: `PutPatientStudy`)
- **HTTPPUT** `CancelInpatientRequisitions` (Method: `CancelInpatientRequisitions`)
- **HTTPPUT** `Doctor` (Method: `Doctor`)
- **HTTPPUT** `PatientScanDone` (Method: `PatientScanDone`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPPUT** `Put` (Method: `Put`)
- **HTTPPOST** `PostReport` (Method: `PostReport`)
- **HTTPGET** `GetImagingReport` (Method: `GetImagingReport`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/radiology`

### Routing / Views
- `/ImagingRequisitionList`
- `/ImagingReportsList`
- `/InpatientList`
- `/WardBilling`
- `/EditDoctors`
- `/**`

### Angular Components
- `radiology-main`
- `rad-edit-doctors-popup`
- `rad-edit-doctors`
- `imaging-reports-list`
- `imaging-requisition-list`
- `ImagingTypeSelector`
- `post-report`
- `view-report`
- `rad-ip-list`
- `rad-wardbilling`

### Angular Services & Endpoints
- `imaging.bl.service.ts`
- `imaging.dl.service.ts`

