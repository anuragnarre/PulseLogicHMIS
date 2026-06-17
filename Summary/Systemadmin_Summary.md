# Systemadmin Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/SystemAdmin`

### API Endpoints
- **HTTPGET** `DatabaseBakupLogs` (Method: `GetDatabaseBakupLogs`)
- **HTTPGET** `AuditTrialDetails` (Method: `GetAuditTrialDetails`)
- **HTTPGET** `AuditList` (Method: `AuditList`)
- **HTTPGET** `LoginInformation` (Method: `GetLoginInformation`)
- **HTTPGET** `IRDInvoiceDetails` (Method: `IRDInvoiceDetails`)
- **HTTPGET** `AllIRDInvoiceDetails` (Method: `AllIRDInvoiceDetails`)
- **HTTPGET** `PharmacyIRDInvoiceDetails` (Method: `PharmacyIRDInvoiceDetails`)
- **HTTPGET** `DatabaseActivity` (Method: `DatabaseActivity`)
- **HTTPGET** `SystemAdmin` (Method: `GetSystemAdmin`)
- **HTTPPOST** `DatabaseBackup` (Method: `DatabaseBackup`)
- **HTTPPOST** `RestoreDatabase` (Method: `RestoreDatabase`)
- **HTTPPOST** `ExportDatabase` (Method: `ExportDatabase`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/system-admin`

### Routing / Views
- `/DatabaseBackup`
- `/DatabaseAudit`
- `/InvoiceDetails`
- `/NewSalesBook`
- `/SalesBook`
- `/AuditTrail`
- `/**`

### Angular Components
- `system-admin-main`
- `audit-trail`
- `database-audit`
- `database-backup`
- `invoice-details`
- `new-sales-book`
- `phrm-sales-book-report`
- `sales-book-report`

### Angular Services & Endpoints
- `system-admin.bl.service.ts`
- `system-admin.dl.service.ts`

