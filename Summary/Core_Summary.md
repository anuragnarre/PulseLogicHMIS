# Core Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Core`

### API Endpoints
- **HTTPGET** `Lookups` (Method: `GetLookups`)
- **HTTPGET** `AppSettings` (Method: `GetAppSettings`)
- **HTTPGET** `EmployeeDatePreference` (Method: `GetEmployeeDatePreference`)
- **HTTPPOST** `EmployeeDatePreference` (Method: `EmployeeDatePreference`)
- **HTTPGET** `TemplateTypes` (Method: `TemplateType`)
- **HTTPGET** `TemplatePrintHtml` (Method: `TemplatePrintHtml`)
- **HTTPGET** `Templates` (Method: `Templates`)
- **HTTPGET** `TemplateFields` (Method: `TemplateFields`)
- **HTTPGET** `FieldMaster` (Method: `FieldMaster`)
- **HTTPGET** `GetSelectedTemplateData` (Method: `GetSelectedTemplateData`)
- **HTTPGET** `GetFieldMasterByTemplateId` (Method: `GetFieldMasterByTemplateId`)
- **HTTPPUT** `ActivateDeactivate` (Method: `ActivateDeactivateTemplate`)
- **HTTPPUT** `UpdateDynamicTemplate` (Method: `UpdateDynamicTemplate`)
- **HTTPPOST** `AddNewTemplate` (Method: `AddNewTemplate`)
- **HTTPPOST** `AddUpdateFieldMapping` (Method: `AddUpdateFieldMapping`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPPUT** `Put` (Method: `Put`)
- **HTTPDELETE** `Delete` (Method: `Delete`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPPUT** `Put` (Method: `Put`)
- **HTTPDELETE** `Delete` (Method: `Delete`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/core`

### Angular Components
- `json-table`
- `question-hrc`
- `question`
- `dyn-template-edit`
- `question-add`
- `question-edit`
- `notification`

### Angular Services & Endpoints
- `notification.bl.service.ts`
- `notification.dl.service.ts`
- `backbutton-disable.service.ts`
- `core.bl.service.ts`
- `core.dl.service.ts`
- `core.service.ts`

