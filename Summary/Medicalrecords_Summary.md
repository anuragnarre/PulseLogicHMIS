# Medicalrecords Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/MedicalRecords`

### API Endpoints
- **HTTPGET** `MRMasterData` (Method: `MRMasterData`)
- **HTTPGET** `PatientTests` (Method: `PatientTests`)
- **HTTPGET** `PatientMrDetails` (Method: `MRDetailsOfPatient`)
- **HTTPGET** `Births` (Method: `BirthList`)
- **HTTPGET** `DeathPatients` (Method: `Deaths`)
- **HTTPGET** `BirthCertificateDetail` (Method: `BirthCertificateDetail`)
- **HTTPGET** `DeathCertificateDetail` (Method: `DeathCertificateDetail`)
- **HTTPGET** `OutPatientsVisitInfo` (Method: `OutPatientsVisitInfo`)
- **HTTPGET** `ICD10ReportingGroup` (Method: `GetICD10ReportingGroup`)
- **HTTPGET** `ICD10DiseaseGroup` (Method: `GetICD10DiseaseGroup`)
- **HTTPGET** `BirthCertificateNumbers` (Method: `AllBirthCertificateNumbers`)
- **HTTPGET** `MRFileNumbers` (Method: `GetMRFileNumbers`)
- **HTTPGET** `DeathCertificatesNumbers` (Method: `DeathCertificateNumbers`)
- **HTTPGET** `ICD10List` (Method: `GetICD10List`)
- **HTTPGET** `OutpatientDiagnosis/{patId}/{patVisitId}` (Method: `GetOutpatientDiagnosisByVisitId`)
- **HTTPGET** `BabyDetails/{patientId}` (Method: `GetBabyDetailsByMotherId`)
- **HTTPGET** `FemalePatientsVisitDetails/{search}` (Method: `GetFemalePatientsVisitDetails`)
- **HTTPGET** `SearchDeadPatient/{searchTxt}` (Method: `GetPatientListWithVisitsId`)
- **HTTPGET** `DeadPatients` (Method: `GetDeadPatients`)
- **HTTPGET** `PatientDeathDetail/{PatientId}` (Method: `GetPatientDeathDetail`)
- **HTTPPOST** `PatientMedicalRecord` (Method: `PostMedicalrecord`)
- **HTTPPOST** `BirthDetails` (Method: `AddBirthDetails`)
- **HTTPPOST** `AddDeathDetails` (Method: `AddDeathDetails`)
- **HTTPPOST** `FinalDiagnosis` (Method: `PostFinalDiagnosis`)
- **HTTPPUT** `Birthdetail` (Method: `PutBirthdetail`)
- **HTTPPUT** `MedicalRecord` (Method: `PutMedicalRecord`)
- **HTTPPUT** `BirthCertificateDetail` (Method: `PutBirthCertificateDetail`)
- **HTTPPUT** `DeathCertificateDetail` (Method: `PutDeathCertificateDetail`)
- **HTTPPUT** `BirthCertificatePrintCount` (Method: `PutBirthCertificatePrintCount`)
- **HTTPPUT** `DeathCertificatePrintCount` (Method: `PutDeathCertificatePrintCount`)
- **HTTPPUT** `DeathDetail` (Method: `PutUpdateDeathDetail`)
- **HTTPGET** `LatestDeathCertificateNumber` (Method: `GetLatestDeathCertificateNumber`)
- **HTTPGET** `ICDReportingGroupForEmergencyPatient` (Method: `GetICDReportingGroupForEmergencyPatient`)
- **HTTPGET** `ICDDiseaseGroupForEmergencyPatient` (Method: `GetICDEmergencyDiseaseGroup`)
- **HTTPGET** `EmergencyPatientsListWithVisitInfo/{fromDate}/{toDate}` (Method: `GetEmergencyPatientsListWithVisitInfo`)
- **HTTPGET** `EmergencyPatientDiagnosisDetail/{patId}/{patVisitId}` (Method: `GetEmergencyPatientDiagnosisDetail`)
- **HTTPPOST** `PostEmergencyFinalDiagnosis` (Method: `PostEmergencyFinalDiagnosis`)
- **HTTPGET** `DischargedPatients` (Method: `DischargedPatients`)
- **HTTPGET** `EthnicGroupStatisticsReports` (Method: `EthnicGroupStatisticsReports`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/medical-records`

### Routing / Views
- `/OutpatientList`
- `/InpatientList`
- `/BirthList`
- `/DeathList`
- `/EmergencyPatientList`
- `/ReportList`
- `/InpatientServicesReport`
- `/LabServicesReport`
- `/DiseaseWiseReport`
- `/OutpatientServicesReport`
- `/OutpatientMorbidityReport`
- `/InpatientMorbidityReport`
- `/HospitalServiceSummary`
- `/HospitalMortalityReport`
- `/EmergencyPatientMorbidityReport`
- `/EthnicGroupStatisticsReport`
- `/**`

### Angular Components
- `medical-records-main`
- `add-birth-details-shared`
- `add-death-details-shared`
- `birth-list`
- `add-birth-details`
- `birth-certificate`
- `death-certificate`
- `death-list`
- `add-death-details`
- `emergency-patient-list`
- `emergency-add-final-diagnosis`
- `inpatient-list`
- `emergency-patient-morbidity-report`
- `ethnic-group-statistics`
- `hospital-mortality`
- `gov-inpatient-outcome-report`
- `gov-laboratory-services`
- `govt-summary-report`
- `mr-summary-add`
- `mr-summary-view`
- `outpatient-list`
- `add-final-diagnosis`

### Angular Services & Endpoints
- `medical-record.service.ts`
- `mr.bl.service.ts`
- `mr.dl.service.ts`

