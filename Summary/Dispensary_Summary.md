# Dispensary Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Dispensary`

### API Endpoints
- **HTTPGET** `Dispensaries` (Method: `Dispensaries`)
- **HTTPGET** `PharmacyStores` (Method: `GetAllPharmacyStores`)
- **HTTPGET** `GetDispensary` (Method: `GetDispensary`)
- **HTTPPOST** `NewDispensary` (Method: `AddDispensary`)
- **HTTPPUT** `PutDispensary` (Method: `UpdateDispensary`)
- **HTTPPUT** `ActivateDeactivate` (Method: `ActivateDeactivateDispensary`)
- **HTTPGET** `TestFileUpload` (Method: `TestFileUpload`)
- **HTTPGET** `TestFileUpload` (Method: `TestFileUpload`)
- **HTTPGET** `GetAll` (Method: `GetAll`)
- **HTTPGET** `GetByDispensaryIdAsync` (Method: `GetByDispensaryIdAsync`)
- **HTTPGET** `GetRequisitionViewByIdAsync` (Method: `GetRequisitionViewByIdAsync`)
- **HTTPGET** `GetItemsForRequisition` (Method: `GetItemsForRequisition`)
- **HTTPGET** `RequisitionDispatchToReceive` (Method: `GetRequisitionDispatchToReceiveAsync`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPPUT** `UpdateDispensary` (Method: `UpdateDispensary`)
- **HTTPPUT** `ReceiveDispatchedItems` (Method: `ReceiveDispatchedItems`)
- **HTTPPUT** `ApproveRequisition` (Method: `ApproveRequisition`)
- **HTTPPUT** `CancelRequisitionItems` (Method: `CancelRequisitionItems`)
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPGET** `GetAllStoresForTransfer` (Method: `GetAllStoresForTransfer`)
- **HTTPGET** `GetDispensariesStock` (Method: `GetDispensariesStock`)
- **HTTPPOST** `Post` (Method: `Post`)

### Services
Backend services encapsulate business logic and data access for Dispensary.
- `DispensaryRequisitionService.cs`
- `DispensaryService.cs`
- `IDispensaryRequisitionService.cs`
- `IDispensaryService.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/dispensary`

### Routing / Views
- `/ActivateDispensary`
- `/Patient`
- `/List`
- `/**`
- `/Prescription`
- `/Sale`
- `/New`
- `/Return`
- `/ReturnList`
- `/ReceiptPrint`
- `/CreditBills`
- `/Settlement`
- `/PendingSettlements`
- `/SettlementReceipts`
- `/ProvisionalReturn`
- `/Dispatch`
- `/SaleCredit`
- `/Stock`
- `/StockDetails`
- `/Requisition`
- `/Add`
- `/View`
- `/ReceiveStock`
- `/Transfer`
- `/PatientConsumptionMain`
- `/PatientConsumption`
- `/ConsumptionReturn`
- `/NewPatientConsumption`
- `/FinalizeInvoice`
- `/ReturnPatientConsumption`
- `/FinalizeConsumption`
- `/FinalizedConsumption`
- `/ActivateCounter`
- `/Reports`
- `/Reports/UserWiseCollectionReport`
- `/Reports/DISPNarcoticsDailySalesReport`
- `/Reports/DISPCashCollectionSummaryReport`
- `/Reports/DISPDailySalesReport`
- `/Reports/SettlementSummaryReport`
- `/Reports/PaymentModeWiseReport`

### Angular Components
- `activate-dispensary`
- `dispensary-main`
- `activate-counter`
- `patient-main`
- `patient-deposit-add`
- `patient-list`
- `prescription-main`
- `prescription-list`
- `reports-main`
- `disp-cash-collection-summary-report`
- `disp-daily-sales-report`
- `disp-userwise-collection-report`
- `disp-narcotis-daily-sales-report`
- `paymentmode-wise-report`
- `settlement-summary-report`
- `sales-main`
- `credit-bills`
- `new-sales`
- `stock-list-pop-up`
- `print-receipt`
- `provisional-return`
- `sales-list`
- `sales-return`
- `manual-sales-return`
- `sales-return-invoice-view`
- `sales-return-list`
- `phrm-settlement-invoice-detail`
- `settlement`
- `duplicate-print-settlement-list`
- `pending-settlement-list`
- `settlement-receipt`
- `stock-main`
- `receive-dispatched-stock`
- `requisition-add`
- `requisition-list`
- `requisition-view`
- `stock-list`
- `transfer-main`
- `transfer-create`
- `transfer-list`
- `transfer-view`

### Angular Services & Endpoints
- `dispensary-requisition.service.ts`
- `transfer-endpoint.service.ts`
- `transfer.service.ts`
- `dispensary-guard.service.ts`
- `dispensary.endpoint.ts`
- `dispensary.service.ts`

