# Clinical Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Clinical`

### API Endpoints
- **HTTPGET** `LatestVitals` (Method: `Vitals`)
- **HTTPGET** `ProviderLongSignature` (Method: `ProviderLongSignature`)
- **HTTPGET** `InputOutput` (Method: `InputOutput`)
- **HTTPGET** `PrescriptionHistory` (Method: `PrescriptionHistory`)
- **HTTPGET** `PatientAllergies` (Method: `PatientAllergies`)
- **HTTPGET** `HomeMedication` (Method: `HomeMedication`)
- **HTTPGET** `MedicationPrescriptions` (Method: `MedicationPrescriptions`)
- **HTTPGET** `ActiveMedicalProblems` (Method: `ActivemedicalProblems`)
- **HTTPGET** `PastMedicalProblems` (Method: `PastMedicalProblems`)
- **HTTPGET** `SurgicalHistory` (Method: `SurgicalHistory`)
- **HTTPGET** `FamilyHistories` (Method: `FamilyHistories`)
- **HTTPGET** `SocialHistory` (Method: `SocialHistory`)
- **HTTPGET** `ReferralSource` (Method: `ReferralSource`)
- **HTTPGET** `ClinicalDetail` (Method: `PatientClinicalDetail`)
- **HTTPGET** `NoteTypes` (Method: `NoteTypes`)
- **HTTPGET** `NotesTemplates` (Method: `NotesTemplates`)
- **HTTPGET** `FreeTextNoteTemplateDetail` (Method: `FreeTextNoteTemplateDetail`)
- **HTTPGET** `ProcedureNoteTemplate` (Method: `ProcedureNoteTemplateDetail`)
- **HTTPGET** `ProgressNoteTemplateDetail` (Method: `ProgressNoteTemplateDetail`)
- **HTTPGET** `HistoryAndPhysicalNoteDetail` (Method: `HistoryAndPhysicalNoteDetail`)
- **HTTPGET** `EmergencyNoteDetail` (Method: `EmergencyNoteDetail`)
- **HTTPGET** `PrescriptionNoteDetail` (Method: `PrescriptionNoteDetail`)
- **HTTPGET** `PatientNotes` (Method: `PatientNotes`)
- **HTTPGET** `OPDGeneralNote` (Method: `OPDGeneralNote`)
- **HTTPGET** `ScannedImages` (Method: `ScannedImages`)
- **HTTPGET** `EyeHistory` (Method: `EyeHistory`)
- **HTTPGET** `EyeEMR` (Method: `EyeEMR`)
- **HTTPGET** `PrescriptionDetails` (Method: `PrescriptionDetails`)
- **HTTPGET** `PrescriptionNote` (Method: `PrescriptionNote`)
- **HTTPGET** `Orders` (Method: `Orders`)
- **HTTPGET** `TemplateDetailsByNoteId` (Method: `TemplateDetailsByNoteId`)
- **HTTPGET** `getClinicalIntakeOutputParameter` (Method: `getClinicalIntakeOutputParameter`)
- **HTTPPOST** `Vitals` (Method: `Vitals`)
- **HTTPPOST** `InputOutput` (Method: `InputOutput`)
- **HTTPPOST** `ScannedEyeImages` (Method: `PostScannedEyeImages`)
- **HTTPPOST** `Allergy` (Method: `Allergy`)
- **HTTPPOST** `HomeMedication` (Method: `HomeMedication`)
- **HTTPPOST** `MedicationPrescription` (Method: `MedicationPrescription`)
- **HTTPPOST** `ActiveMedicalProblem` (Method: `ActiveMedicalProblem`)
- **HTTPPOST** `PastMedicalProblem` (Method: `PastMedicalProblem`)
- **HTTPPOST** `FamilyHistory` (Method: `FamilyHistory`)
- **HTTPPOST** `SurgicalHistory` (Method: `SurgicalHistory`)
- **HTTPPOST** `SocialHistory` (Method: `SocialHistory`)
- **HTTPPOST** `ReferralSource` (Method: `ReferralSource`)
- **HTTPPOST** `OPDGeneralNote` (Method: `OPDGeneralNote`)
- **HTTPPOST** `EyeMasterDetails` (Method: `EyeMasterDetail`)
- **HTTPPOST** `ProcedureNoteTemplate` (Method: `ProcedureNoteTemplate`)
- **HTTPPOST** `ProgressNoteTemplateDetail` (Method: `ProgressNoteTemplate`)
- **HTTPPOST** `FreeTextNoteTemplateDetail` (Method: `PostFreetextNoteTemplateDetail`)
- **HTTPPOST** `HistoryAndPhysicalNoteDetail` (Method: `HistoryAndPhysicalNote`)
- **HTTPPOST** `EmergencyNote` (Method: `EmergencyNote`)
- **HTTPPOST** `DischargeNote` (Method: `DischargeNote`)
- **HTTPPOST** `PrescriptionMasterSlip` (Method: `PrescriptionMasterSlip`)
- **HTTPPOST** `PrescriptionNote` (Method: `PrescriptionNote`)
- **HTTPPOST** `PatientFiles` (Method: `PostPatient`)
- **HTTPPUT** `ResolveActiveMedicalProblem` (Method: `ResolveActiveMedicalProblem`)
- **HTTPPUT** `ActiveMedicalProblem` (Method: `PutActiveMedicalProblem`)
- **HTTPPUT** `DeactivatePatientImage` (Method: `DeactivatePatientImage`)
- **HTTPPUT** `Vitals` (Method: `UpdateVitals`)
- **HTTPPUT** `Allergy` (Method: `UpdateAllergy`)
- **HTTPPUT** `FamilyHistory` (Method: `UpdateFamilyHistory`)
- **HTTPPUT** `HomeMedication` (Method: `UpdateHomeMedication`)
- **HTTPPUT** `InputOutput` (Method: `UpdateInputOutput`)
- **HTTPPUT** `MedicationPrescription` (Method: `UpdateMedicationPrescription`)
- **HTTPPUT** `PastMedical` (Method: `PastMedical`)
- **HTTPPUT** `SocialHistory` (Method: `UpdateSocialHistory`)
- **HTTPPUT** `ReferralSource` (Method: `UpdateReferralSource`)
- **HTTPPUT** `SurgicalHistory` (Method: `UpdateSurgicalHistory`)
- **HTTPPUT** `OPDGeneralNote` (Method: `UpdateOPDGeneralNote`)
- **HTTPPUT** `ProgressNoteTemplateDetail` (Method: `ProgressNoteTemplateDetail`)
- **HTTPPUT** `ProcedureNoteTemplate` (Method: `ProcedureNoteTemplateDetail`)
- **HTTPPUT** `FreeTextNoteTemplateDetail` (Method: `PutFreetextNoteTemplateDetail`)
- **HTTPPUT** `DischargeNoteTemplate` (Method: `DischargeNoteTemplateDetail`)
- **HTTPPUT** `EmergencyNoteDetail` (Method: `EmergencyNoteDetail`)
- **HTTPPUT** `PrescriptionNote` (Method: `UpdatePrescriptionNote`)
- **HTTPPUT** `HistoryAndPhysicalNoteDetail` (Method: `UpdateHistoryAndPhysicalNote`)
- **HTTPPUT** `EyeMasterDetail` (Method: `UpdateEyeMasterDetail`)
- **HTTPGET** `bloodsugar` (Method: `getpatientBloodSugarInfo`)
- **HTTPGET** `patient-admission-info` (Method: `AdmitttedPatientInfo`)
- **HTTPPOST** `bloodsugar` (Method: `postBloodSugar`)
- **HTTPGET** `ConsultationRequestsByPatientVisitId` (Method: `ConsultationRequestsByPatientVisitId`)
- **HTTPGET** `GetAllApptDepartment` (Method: `GetAllApptDepartment`)
- **HTTPGET** `GetAllAppointmentApplicableDoctor` (Method: `GetAllAppointmentApplicableDoctor`)
- **HTTPPOST** `AddNewConsultationRequest` (Method: `AddNewConsultationRequest`)
- **HTTPPUT** `ResponseConsultationRequest` (Method: `ResponseConsultationRequest`)
- **HTTPGET** `DietTypes` (Method: `DietTypes`)
- **HTTPGET** `InpatientListWithDietDetail` (Method: `InpatientListWithDietDetail`)
- **HTTPGET** `PatientDietHistory` (Method: `PatientDietHistory`)
- **HTTPPOST** `AddPatientDietType` (Method: `AddPatientDietType`)
- **HTTPPOST** `api/Scribe/Transcribe` (Method: `Transcribe`)

### Services
Backend services encapsulate business logic and data access for Clinical.
- `ClinicalMedicationService.cs`
- `ClinicalVitalsService.cs`
- `IClinicalMedicationService.cs`
- `IClinicalVitalsService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/clinical-notes`

### Angular Components
- `notes-main`
- `assessment-plan`
- `freenotes`
- `notes-list`
- `objective-note`
- `opd-general-note`
- `opd-ortho-note`
- `clinical-prescription-note`
- `prescription-note`
- `view-clinical-prescription-note`
- `subjective-note`
- `emergency-note`
- `view-emergency-note`
- `freetext`
- `history-and-physical-note`
- `view-history-and-physical-note`
- `procedure-note`
- `progress-note`

### Angular Services & Endpoints
- `note-template.bl.service.ts`

