# Doctors Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Doctors`

### API Endpoints
- **HTTPGET** `PatientOverview` (Method: `PatientOverview`)
- **HTTPGET** `OtherRequestsOfPatient` (Method: `OtherRequestsOfPatient`)
- **HTTPGET** `TodaysVisits` (Method: `TodaysVisitList`)
- **HTTPGET** `PastVisits` (Method: `PastVisits`)
- **HTTPGET** `DepartmentVisits` (Method: `DoctorDepartmentVisit`)
- **HTTPGET** `EmployeeDepartment` (Method: `DepartmentByEmployeeId`)
- **HTTPGET** `PatientVisitTypes` (Method: `PatientVisitType`)
- **HTTPPOST** `ConcludeVisit` (Method: `ConcludeVisit`)
- **HTTPPUT** `ReassignProvider` (Method: `ReassignProvider`)
- **HTTPPUT** `ChangeProvider` (Method: `ChangeProvider`)
- **HTTPGET** `VisitDetails` (Method: `VisitDetails`)
- **HTTPPOST** `VisitDetails` (Method: `PostVisitDetils`)
- **HTTPPUT** `VisitDetails` (Method: `UpdateVisitDetails`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/doctors`

### Angular Components
- `doctors-main`
- `doctor-dashboard`
- `ipd-main`
- `doctors-notes`
- `opd-visit-summary`
- `outpatient-main`
- `op-new-patient`
- `opd-record`
- `in-patient-discharge-summary`
- `patient-overview-main`
- `patient-overview`
- `patient-visit-history`
- `referral-source-add`
- `referral-source-list`
- `doctor-summary`
- `visit-summary-create`
- `visit-summary-history`
- `visit-summary-main`
- `visit-summary`

### Angular Services & Endpoints
- `doctors.bl.service.ts`
- `doctors.dl.service.ts`

