# Patient Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Patient`

### API Endpoints
- **HTTPGET** `GetPatientByGUID` (Method: `GetPatientByGUID`)
- **HTTPGET** `GetPatientCurrentPriceCategory` (Method: `GetPatientCurrentPriceCategory`)
- **HTTPGET** `GetCastEthnicGroupList` (Method: `GetCastEthnicGroupList`)
- **HTTPGET** `PatientById` (Method: `PatientById`)
- **HTTPGET** `PatientByCode` (Method: `PatientByCode`)
- **HTTPGET** `LightPatientById` (Method: `LightPatientById`)
- **HTTPGET** `MatchingPatients` (Method: `MatchingPatients`)
- **HTTPGET** `PatientDocuments` (Method: `PatientDocuments`)
- **HTTPGET** `PatientWithVisitInfo` (Method: `PatientWithVisitInfo`)
- **HTTPGET** `IPDPatientSearch` (Method: `IPDPatientSearch`)
- **HTTPGET** `PatientLastVisitContext` (Method: `PatientLastVisitContext`)
- **HTTPGET** `PatientProfilePicture` (Method: `PatientProfilePicture`)
- **HTTPGET** `InsuranceProviders` (Method: `InsuranceProviders`)
- **HTTPGET** `NewDialysicCode` (Method: `NewDialysicCode`)
- **HTTPGET** `HealthCardStatus` (Method: `HealthCardStatus`)
- **HTTPGET** `MembershipTypes` (Method: `MembershipTypes`)
- **HTTPGET** `AdmittedPatients` (Method: `AdmittedPatients`)
- **HTTPGET** `SearchPatient` (Method: `SearchPatient`)
- **HTTPGET** `SearchRegisteredPatient` (Method: `SearchRegisteredPatient`)
- **HTTPGET** `SearchPatientForNewVisit` (Method: `SearchPatientForNewVisit`)
- **HTTPGET** `PatientDetailForVaccination` (Method: `PatientDetailForVaccination`)
- **HTTPPOST** `PostPatient` (Method: `PostPatient`)
- **HTTPPOST** `PatientFiles` (Method: `PatientFiles`)
- **HTTPPOST** `PatientProfilePicture` (Method: `PatientProfilePicture`)
- **HTTPPOST** `PatientHealthCard` (Method: `PatientHealthCard`)
- **HTTPPOST** `NeighbourhoodCard` (Method: `NeighbourhoodCard`)
- **HTTPPOST** `GovInsurancePatient` (Method: `GovInsurancePatient`)
- **HTTPPOST** `BillingOutPatient` (Method: `BillingOutPatient`)
- **HTTPPUT** `GovInsurancePatient` (Method: `UpdateGovInsurancePatient`)
- **HTTPPUT** `PutPatient` (Method: `PutPatient`)
- **HTTPDELETE** `Delete` (Method: `Delete`)
- **HTTPGET** `DownloadFile` (Method: `Download`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/patients`

### Angular Components
- `patients-main`
- `patient-duplicate-warning-box`
- `patient-health-card`
- `hams-health-card`
- `patient-neighbour-card-backup`
- `patient-neighbour-card`
- `patient-hisotry`
- `patient-list`
- `patient-upload-files`
- `profile-pic`
- `patient-registration-main`
- `address`
- `patient-basic-info`
- `guarantor`
- `insurance-info`
- `kin-emergency-contact`

### Angular Services & Endpoints
- `patient.service.ts`
- `patients.bl.service.ts`
- `patients.dl.service.ts`

