# Vaccination Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Vaccination`

### API Endpoints
- **HTTPGET** `GetAllVaccinationPatient` (Method: `GetAllVaccinationPatient`)
- **HTTPGET** `GetVaccinationPatientDetailById` (Method: `GetVaccinationPatientDetailById`)
- **HTTPGET** `GetAllVaccinesOfPatientByPatientId` (Method: `GetAllVaccinesOfPatientByPatientId`)
- **HTTPGET** `GetAllVaccineWiseDoseMapped` (Method: `GetAllVaccineWiseDoseMapped`)
- **HTTPGET** `GetVaccinationIntegratedreport` (Method: `GetVaccinationIntegratedreport`)
- **HTTPGET** `GetAllBabyPatient` (Method: `GetAllBabyPatient`)
- **HTTPGET** `GetLatestVaccineRegistrationNum` (Method: `GetLatestVaccineRegistrationNum`)
- **HTTPGET** `GetExistingVaccRegistrationData` (Method: `GetExistingVaccRegistrationData`)
- **HTTPGET** `GetCastEthnicGroupList` (Method: `GetCastEthnicGroupList`)
- **HTTPPOST** `AddVaccPatient` (Method: `AddVaccPatient`)
- **HTTPPOST** `AddPatientVaccineationDetail` (Method: `AddPatientVaccineationDetail`)
- **HTTPPUT** `UpdateVaccinationRegnumberOfPatient` (Method: `UpdateVaccinationRegnumberOfPatient`)
- **HTTPGET** `GetPatientAndVisitInfo` (Method: `GetPatientAndVisitInfo`)
- **HTTPPOST** `PostFollowupVisit` (Method: `PostFollowupVisit`)
- **HTTPGET** `GetDailyAppointmentReport` (Method: `GetDailyAppointmentReport`)

### Services
Backend services encapsulate business logic and data access for Vaccination.
- `IVaccinationService.cs`
- `VaccinationService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/vaccination`

### Routing / Views
- `/PatientList`
- `/Reports`
- `/IntegratedReport`
- `/AppointmentDetailsReport`
- `/**`

### Angular Components
- `vaccination-main`
- `vaccination-followup-add`
- `vaccination-patient-list`
- `vaccination-report.main`
- `vaccination-appointment-details-report`
- `patient-vaccine-report`
- `vacc-select`
- `vaccination-sticker`
- `vaccination-patient-registration`

### Angular Services & Endpoints
- `vaccination.bl.service.ts`
- `vaccination.dl.service.ts`
- `vaccination.service.ts`

