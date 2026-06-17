# Queuemanagement Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/QueueManagement`

### API Endpoints
- **HTTPGET** `GetAllApptDepartment` (Method: `GetAllApptDepartment`)
- **HTTPGET** `GetAppointmentData` (Method: `GetAppointmentData`)
- **HTTPGET** `GetAllAppointmentApplicableDoctor` (Method: `GetAllAppointmentApplicableDoctor`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPPUT** `updateQueueStatus` (Method: `updateQueueStatus`)

### Services
Backend services encapsulate business logic and data access for Queuemanagement.
- `IQueueManagementService.cs`
- `QueueManagementService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/queue-management`

### Routing / Views
- `/Opd`
- `/**`

### Angular Components
- `opd`

### Angular Services & Endpoints
- `Qmgnt.bl.service.ts`
- `Qmgnt.dl.service.ts`
- `Qmgnt.service.ts`

