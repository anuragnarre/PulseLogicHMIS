# Incentive Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Incentive`

### API Endpoints
- **HTTPGET** `Profiles` (Method: `Profiles`)
- **HTTPGET** `Categories` (Method: `Categories`)
- **HTTPGET** `ProfileItems` (Method: `ProfileItems`)
- **HTTPGET** `ProfileItemsMapping` (Method: `ProfileItemsMapping`)
- **HTTPGET** `EmployeesIncentiveInfo` (Method: `EmployeesIncentiveInfo`)
- **HTTPGET** `IncentiveItems` (Method: `IncentiveItems`)
- **HTTPGET** `EmployeeBillItems` (Method: `EmployeeBillItems`)
- **HTTPGET** `TransactionItems` (Method: `TransactionItems`)
- **HTTPGET** `TransactionInvoices` (Method: `TransactionInvoices`)
- **HTTPGET** `TransactionInvoiceItems` (Method: `TransactionInvoiceItems`)
- **HTTPGET** `FractionOfBillTransactionItems` (Method: `FractionOfBillTransactionItems`)
- **HTTPGET** `IncentiveApplicableDoctors` (Method: `IncentiveApplicableDoctors`)
- **HTTPPOST** `Profile` (Method: `AddProfile`)
- **HTTPPOST** `MapProfileItems` (Method: `MapProfileItems`)
- **HTTPPOST** `MapEmployeeBillItems` (Method: `MapEmployeeBillItems`)
- **HTTPPOST** `ActivateDeactivateEmployeeSetup` (Method: `ActivateDeactivateEmployeeSetup`)
- **HTTPPOST** `EmployeeBillItems` (Method: `EmployeeBillItem`)
- **HTTPPOST** `ProfileBillItemMap` (Method: `ProfileBillItemMap`)
- **HTTPPOST** `RemoveBillItem` (Method: `RemoveBillItem`)
- **HTTPPOST** `RemoveBillItemFromProfileMap` (Method: `RemoveBillItemFromProfileMap`)
- **HTTPPOST** `ItemsGroupDistribution` (Method: `ItemsGroupDistribution`)
- **HTTPPOST** `FractionItems` (Method: `FractionItems`)
- **HTTPPOST** `Transactions` (Method: `Transactions`)
- **HTTPPOST** `ActivateDeactivateProfile` (Method: `ActivateDeactivateProfile`)
- **HTTPPUT** `Profile` (Method: `UpdateProfile`)
- **HTTPPUT** `BillTransactionItems` (Method: `BillTransactionItems`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPPUT** `Put` (Method: `Put`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/incentive`

### Routing / Views
- `/Setting`
- `/ProfileManage`
- `/EmployeeProfileMap`
- `/EmployeeItemsSetup`
- `/EmployeeItemsSetupOld`
- `/**`
- `/Reports`
- `/Transactions`
- `/TransactionReport`
- `/PaymentReport`
- `/PatientVsService`
- `/HospitalIncomeReport`
- `/ReferralSummaryReport`
- `/InvoiceItemLevel`
- `/InvoiceLevel`
- `/BillSync`
- `/MakePayment`

### Angular Components
- `incentive-main`
- `edit-incentive-txn-item`
- `incentive-payment-Info`
- `inctv-txn-items-list`
- `inctv-txn-Main`
- `hospital-income-service-department-wise`
- `hospital-income`
- `incentive-patientVsService-report`
- `incentive-payment-report-summary`
- `incentive-item-group-summary`
- `incentive-item-summary-report`
- `incentive-report-summary`
- `profile-item-mapping`
- `profile-manage`
- `setting-main`
- `view-emp-incentive-settings`
- `employee-items-setup-main`
- `employee-items-setup`
- `inctv-edit-fraction`
- `inctv-billtxnitems-list`

### Angular Services & Endpoints
- `incentive.bl.service.ts`
- `incentive.dl.service.ts`

