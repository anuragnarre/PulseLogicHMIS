# Emergency Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Emergency`

### API Endpoints
- **HTTPGET** `LatestEmergencyNumberAndModeOfArrival` (Method: `LatestEmergencyNumberAndModeOfArrival`)
- **HTTPGET** `EmergencyPatients` (Method: `EmergencyPatients`)
- **HTTPGET** `TriagedPatients` (Method: `TriagedPatients`)
- **HTTPGET** `LamaPatients` (Method: `LamaPatients`)
- **HTTPGET** `ExistingPatients` (Method: `ExistingPatients`)
- **HTTPGET** `TransferredPatients` (Method: `TransferredPatients`)
- **HTTPGET** `DischargedPatients` (Method: `DischargedPatients`)
- **HTTPGET** `AdmittedPatients` (Method: `AdmittedPatients`)
- **HTTPGET** `DeadPatients` (Method: `DeadPatients`)
- **HTTPGET** `DischargeOnRequestPatients` (Method: `DischargeOnRequestPatients`)
- **HTTPGET** `Countries` (Method: `Countries`)
- **HTTPGET** `CountrySubDivisions` (Method: `CountrySubDivisions`)
- **HTTPGET** `Doctors` (Method: `Doctors`)
- **HTTPGET** `DischargeSummary` (Method: `DischargeSummary`)
- **HTTPGET** `MatchingPatient` (Method: `FindMatchingPatient`)
- **HTTPGET** `ConsentForm` (Method: `ConsentFormhingPatient`)
- **HTTPGET** `DownloadFile` (Method: `Download`)
- **HTTPPOST** `UploadPatientConsentForm` (Method: `UploadPatientConsentForm`)
- **HTTPPOST** `DischargeSummary` (Method: `DischargeSummary`)
- **HTTPPOST** `RegisterPatient` (Method: `RegisterPatient`)
- **HTTPPUT** `TriagedCode` (Method: `TriagedCode`)
- **HTTPPUT** `LeaveAgainstMedicalAdvice` (Method: `LeaveAgainstMedicalAdvice`)
- **HTTPPUT** `UndoTriageOfPatient` (Method: `UndoTriageOfPatient`)
- **HTTPPUT** `PerformerDetail` (Method: `UpdatePerformer`)
- **HTTPPUT** `ERDischargeSummary` (Method: `UpdateERDischargeSummary`)
- **HTTPPUT** `Patient` (Method: `updatePatient`)
- **HTTPPUT** `CosentForm` (Method: `UpdateConcentFile`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/emergency`

### Routing / Views
- `/Dashboard`
- `/NewPatients`
- `/TriagePatients`
- `/FinalizedPatients`
- `/Lama-Patients`
- `/Transferred-Patients`
- `/Discharged-Patients`
- `/Admitted-Patients`
- `/Death-Patients`
- `/Dor-Patients`
- `/**`
- `/BedInformations`
- `/PatientOverviewMain`
- `/PatientOverview`
- `/Clinical`
- `/WardBilling`
- `/DrugsRequest`
- `/Notes`

### Angular Components
- `emergency-main`
- `bed-informations`
- `add-er-discharge-summary`
- `er-discharge-list`
- `er-discharge-summary`
- `view-er-discharge-summary`
- `er-wardbilling`
- `er-admitted-patient-list`
- `er-death-patient-list`
- `er-discharged-patient-list`
- `er-dor-patient-list`
- `er-finalized-patients`
- `er-lama-patient-list`
- `er-transferred-patient-list`
- `er-patient-list`
- `upload-consent`
- `er-patient-registration`
- `select-patient-case`
- `assign-doctor`
- `er-lama`
- `er-triage-actions`
- `er-triage-patient-list`

### Angular Services & Endpoints
- `emergency.bl.service.ts`
- `emergency.dl.service.ts`
- `emergency.service.ts`

