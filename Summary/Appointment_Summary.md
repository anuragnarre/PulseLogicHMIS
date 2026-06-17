# Appointment Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Appointment`

### API Endpoints
- **HTTPGET** `Appointments` (Method: `Appointments`)
- **HTTPGET** `CheckClashingAppointment` (Method: `CheckClashingAppointment`)
- **HTTPGET** `AppointmentApplicableDepartments` (Method: `AppointmentApplicableDepartments`)
- **HTTPGET** `MembershipDetail` (Method: `MembershipDetail`)
- **HTTPGET** `PatientsWithAppointments` (Method: `PatientsWithAppointments`)
- **HTTPPOST** `AddAppointment` (Method: `AddAppointment`)
- **HTTPPUT** `UpdatePatientInAppointment` (Method: `UpdatePatientInAppointment`)
- **HTTPPUT** `AppointmentStatus` (Method: `AppointmentStatus`)
- **HTTPPUT** `UpdateAppointment` (Method: `PutAppointment`)
- **HTTPPUT** `AppointmentInformation` (Method: `UpdateAppointmentInformation`)
- **HTTPGET** `CheckExistingAppointment` (Method: `CheckExistingAppointment`)
- **HTTPGET** `VisitsByClaimCode` (Method: `VisitsByClaimCode`)
- **HTTPGET** `PatientVisitStickerInfo` (Method: `PatientVisitStickerInfo`)
- **HTTPGET** `DepartmentOfIpdVisit` (Method: `DepartmentOfIpdVisit`)
- **HTTPGET** `PatientVisitHistory` (Method: `PatientVisitHistory`)
- **HTTPGET** `PatientTodaysVisits` (Method: `PatientTodaysVisits`)
- **HTTPGET** `PatientVisitsWithDoctors` (Method: `PatientVisitsWithDoctors`)
- **HTTPGET** `PatientCurrentVisitContext` (Method: `PatientCurrentVisitContext`)
- **HTTPGET** `PatientVisitContextForProvisionalPayment` (Method: `PatientVisitContextForProvisionalPayment`)
- **HTTPGET** `DoctorNewOpdBillingItems` (Method: `DoctorNewOpdBillingItems`)
- **HTTPGET** `DepartmentNewOpdBillingItems` (Method: `DepartmentNewOpdBillingItems`)
- **HTTPGET** `DoctorFollowupBillingItems` (Method: `DoctorFollowupBillingItems`)
- **HTTPGET** `DepartmentFollowupBillingItems` (Method: `DepartmentFollowupBillingItems`)
- **HTTPGET** `DepartmentOldPatientBillingItems` (Method: `DepartmentOldPatientBillingItems`)
- **HTTPGET** `DoctorOldPatientBillingItems` (Method: `DoctorOldPatientBillingItems`)
- **HTTPGET** `DoctorOpdReferralBillingItems` (Method: `DoctorOpdReferralBillingItems`)
- **HTTPGET** `AppointmentApplicableDoctors` (Method: `AppointmentApplicableDoctors`)
- **HTTPGET** `VisitsSignedByDoctor` (Method: `VisitsSignedByDoctor`)
- **HTTPGET** `PatientHealthCardWithBillInfo` (Method: `PatientHealthCardWithBillInfo`)
- **HTTPGET** `ListVisits` (Method: `ListVisits`)
- **HTTPGET** `VisitsByStatus` (Method: `VisitsByStatus`)
- **HTTPGET** `GetMemberInformationByScheme` (Method: `MemberInformationByScheme`)
- **HTTPGET** `GetPatientCreditLimitsByScheme` (Method: `PatientCreditLimitsByScheme`)
- **HTTPGET** `GetLatestClaimCode` (Method: `GetLatestClaimCodeForAutoClaimCodeGeneration`)
- **HTTPPOST** `NewVisit` (Method: `NewVisit`)
- **HTTPPOST** `VisitFromOnlineAppointment` (Method: `VisitFromOnlineAppointment`)
- **HTTPPOST** `VisitFromBilling` (Method: `VisitFromBilling`)
- **HTTPPOST** `VisitForFreeReferral` (Method: `VisitForFreeReferral`)
- **HTTPPOST** `VisitForFreeFollowup` (Method: `VisitForFreeFollowup`)
- **HTTPPOST** `VisitForPaidFollowup` (Method: `VisitForPaidFollowup`)
- **HTTPPOST** `DefaultVisitCreate` (Method: `DefaultVisitCreate`)
- **HTTPPUT** `UpdateBillStatus` (Method: `UpdateBillStatus`)
- **HTTPPUT** `UpdateSignedStatus` (Method: `UpdateSignedStatus`)
- **HTTPGET** `GetDependentIdDetails` (Method: `GetDependentIdDetails`)
- **HTTPPUT** `UpdateDendentId` (Method: `UpdateDendentId`)
- **HTTPGET** `GetRank` (Method: `GetRank`)
- **HTTPPOST** `PostRank` (Method: `PostRank`)

### Services
Backend services encapsulate business logic and data access for Appointment.

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/appointments`

### Routing / Views
- `/Visit`
- `/ListAppointment`
- `/CreateAppointment`
- `/ListVisit`
- `/PatientSearch`
- `/SSFClaim`
- `/OnlineAppointment`
- `/PendingList`
- `/CompletedList`
- `/**`

### Angular Components
- `appointments-main`
- `appointment-list`
- `appointment-create`
- `followup-visit`
- `visit-list`
- `opd-sticker-print`
- `patient-search`
- `free-referral-visit`
- `registration-scheme-select`
- `transfer-visit`
- `visit-billing-info`
- `visit-info`
- `visit-main`
- `visit-patient-info`

### Angular Services & Endpoints
- `appointment.bl.service.ts`
- `appointment.dl.service.ts`
- `appointment.service.ts`
- `visit.bl.service.ts`
- `visit.dl.service.ts`
- `visit.service.ts`

