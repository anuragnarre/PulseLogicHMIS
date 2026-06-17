# Helpdesk Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Helpdesk`

### API Endpoints
- **HTTPGET** `EmployeesInfo` (Method: `EmployeesInfo`)
- **HTTPGET** `BedsInfo` (Method: `BedsInfo`)
- **HTTPGET** `BedPatientInfos_Old` (Method: `BedPatientInfos_Old`)
- **HTTPGET** `WardsInfo` (Method: `WardsInfo`)
- **HTTPGET** `BedOccupancyOfWards` (Method: `BedOccupancyOfWards`)
- **HTTPGET** `BedsWithPatientsInfo` (Method: `BedsWithPatientsInfo`)
- **HTTPGET** `Get` (Method: `Get`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/helpdesk`

### Routing / Views
- `/EmployeeInformation`
- `/BedInformation`
- `/WardInformation`
- `/QueueInformation`
- `/**`

### Angular Components
- `helpdesk-main`
- `bed-info`
- `employee-info`
- `ward-info`

### Angular Services & Endpoints
- `helpdesk.bl.service.ts`
- `helpdesk.dl.service.ts`

