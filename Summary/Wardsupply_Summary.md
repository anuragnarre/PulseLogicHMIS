# Wardsupply Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/WardSupply`

### API Endpoints
- **HTTPGET** `GetFixedAssetStockBySubStoreId` (Method: `GetFixedAssetStockBySubStoreId`)
- **HTTPGET** `GetCapitalGoodsItemList` (Method: `GetCapitalGoodsItemList`)
- **HTTPGET** `GetSubstoreAssetRequistionList` (Method: `GetSubstoreAssetRequistionList`)
- **HTTPGET** `GetSubstoreAssetRequistionItemsById` (Method: `GetSubstoreAssetRequistionItemsById`)
- **HTTPGET** `GetSubstoreAssetRequistionListByStoreId` (Method: `GetSubstoreAssetRequistionListByStoreId`)
- **HTTPGET** `GetSubstoreAssetReturnList` (Method: `GetSubstoreAssetReturnList`)
- **HTTPGET** `dispatchview` (Method: `dispatchview`)
- **HTTPGET** `dispatchviewbyDispatchId` (Method: `dispatchviewbyDispatchId`)
- **HTTPGET** `GetSubstoreAssetReturnListByStoreId` (Method: `GetSubstoreAssetReturnListByStoreId`)
- **HTTPGET** `GetSubstoreAssetReturnItemsById` (Method: `GetSubstoreAssetReturnItemsById`)
- **HTTPGET** `GetFixedAssetStockByStoreId` (Method: `GetFixedAssetStockByStoreId`)
- **HTTPPUT** `SendStockToCssd` (Method: `SendStockToCssd`)
- **HTTPGET** `~/api/WardSupplyAssets/GetRequisitionDetailsForDispatch/{RequisitionId}` (Method: `GetRequisitionDetailsForDispatch`)
- **HTTPPOST** `~/api/WardSupplyAssets/PostStoreDispatch` (Method: `PostStoreDispatch`)
- **HTTPPOST** `~/api/WardSupplyAssets/PostDirectDispatch` (Method: `PostDirectDispatch`)
- **HTTPGET** `~/api/WardSupply/GetFixedAssetDispatchListForItemReceive/{RequisitionId}` (Method: `GetFixedAssetDispatchListForItemReceive`)
- **HTTPPOST** `AssetRequisition` (Method: `AssetRequisition`)
- **HTTPGET** `GetInventoryItemsByStoreId` (Method: `GetInventoryItemsByStoreId`)
- **HTTPGET** `GetInventorySubStoreItemsByStoreIdForReturn` (Method: `GetInventorySubStoreItemsByStoreIdForReturn`)
- **HTTPGET** `GetInventoryItemsForPatConsumptionByStoreId` (Method: `GetInventoryItemsForPatConsumptionByStoreId`)
- **HTTPGET** `GetInventoryPatConsumptionItemlistById` (Method: `GetInventoryPatConsumptionItemlistById`)
- **HTTPGET** `GetInventoryConsumptionList` (Method: `GetInventoryConsumptionList`)
- **HTTPGET** `GetInventoryPatientConsumptionReceiptList/{StoreId}/{FromDate}/{ToDate}` (Method: `GetInventoryPatientConsumptionReceiptList`)
- **HTTPGET** `GetDispatchListForItemReceive/{RequisitionId}` (Method: `GetDispatchListForItemReceive`)
- **HTTPGET** `WARDStockItemsReport` (Method: `WARDStockItemsReport`)
- **HTTPGET** `WARDRequisitionReport` (Method: `WARDRequisitionReport`)
- **HTTPGET** `WARDBreakageReport` (Method: `WARDBreakageReport`)
- **HTTPGET** `WARDInternalConsumptionReport` (Method: `WARDInternalConsumptionReport`)
- **HTTPGET** `WARDConsumptionReport` (Method: `WARDConsumptionReport`)
- **HTTPGET** `WARDTransferReport` (Method: `WARDTransferReport`)
- **HTTPGET** `RequisitionDispatchReport` (Method: `RequisitionDispatchReport`)
- **HTTPGET** `TransferReport` (Method: `TransferReport`)
- **HTTPGET** `ConsumptionReport` (Method: `ConsumptionReport`)
- **HTTPGET** `GetSubstoreRequistionList` (Method: `GetSubstoreRequistionList`)
- **HTTPGET** `GetSubstoreAssetReturnList` (Method: `GetSubstoreAssetReturnList`)
- **HTTPGET** `GetSubstoreAssetReturnItemsById` (Method: `GetSubstoreAssetReturnItemsById`)
- **HTTPPOST** `PostConsumption` (Method: `PostConsumption`)
- **HTTPPOST** `PostInventoryConsumption` (Method: `PostInventoryConsumption`)
- **HTTPPOST** `PostInternalConsumption` (Method: `PostInternalConsumption`)
- **HTTPPOST** `PostTransferInventoryStock` (Method: `PostTransferInventoryStock`)
- **HTTPPOST** `PostTransferBackToInventory` (Method: `PostTransferBackToInventory`)
- **HTTPPOST** `PostBreakageStock` (Method: `PostBreakageStock`)
- **HTTPPOST** `PostInvPatientConsumption` (Method: `PostInvPatientConsumption`)
- **HTTPPOST** `ReturnStockToPharmacy` (Method: `ReturnStockToPharmacy`)
- **HTTPPOST** `TransferStock` (Method: `TransferStock`)
- **HTTPPOST** `WardInventoryReturn` (Method: `WardInventoryReturn`)
- **HTTPPUT** `UpdateDispatchedItemsReceiveStatus/{DispatchId}` (Method: `UpdateDispatchedItemsReceiveStatus`)
- **HTTPPUT** `PutInternalConsumption` (Method: `PutInternalConsumption`)
- **HTTPPUT** `UpdateRequisition` (Method: `UpdateRequisition`)
- **HTTPPUT** `PutPatientConsumption` (Method: `PutPatientConsumption`)
- **HTTPPOST** `UpdateReconciledStockFromExcelFile` (Method: `UpdateReconciledStockFromExcelFile`)
- **HTTPGET** `ExportStocksForReconciliationToExcel` (Method: `ExportStocksForReconciliationToExcel`)
- **HTTPGET** `GetItemsForRequisition` (Method: `GetItemsForRequisition`)
- **HTTPGET** `GetItemSubCategory` (Method: `GetItemSubCategory`)
- **HTTPPOST** `Post` (Method: `Post`)
- **HTTPGET** `GetDispatchedItemToReceive` (Method: `DispatchItemToReceive`)
- **HTTPGET** `GetWardAvailableStock` (Method: `GetWardAvailableStock`)
- **HTTPGET** `Verifiers` (Method: `GetVerifiers`)

### Services
Backend services encapsulate business logic and data access for Wardsupply.

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/wardsupply`

### Routing / Views
- `/Pharmacy`
- `/Requisition`
- `/Stock`
- `/Consumption`
- `/ConsumptionItem`
- `/ConsumptionDetails`
- `/InternalConsumption`
- `/InternalConsumptionList`
- `/InternalConsumptionDetails`
- `/PharmacyTransfer`
- `/Reports`
- `/Reports/StockReport`
- `/Reports/RequisitionReport`
- `/Reports/DispatchReport`
- `/Reports/ConsumptionReport`
- `/Reports/InternalConsumptionReport`
- `/Reports/BreakageReport`
- `/Reports/TransferReport`
- `/**`
- `/Inventory`
- `/InventoryRequisitionList`
- `/InventoryRequisitionItem`
- `/InventoryRequisitionDetails`
- `/ReceiveStock`
- `/Return`
- `/ConsumptionList`
- `/ConsumptionAdd`
- `/PatientConsumption`
- `/PatientConsumptionList`
- `/PatientConsumptionAdd`
- `/Reports/RequisitionDispatchReport`
- `/FixedAsset`
- `/List`
- `/View`
- `/RequisitionDispatch`

### Angular Components
- `consumption-list`
- `consumption`
- `internal-consumption-details`
- `internal-consumption-list`
- `internal-consumption`
- `pharmacy-transfer`
- `pharmacy-ward`
- `stock`
- `wardsupply-main`
- `wardsupply-pharmacy-stock`
- `inventory-ward`
- `inventory-ward-consumption-list`
- `inventory-ward-consumption`
- `inventory-ward-patient-consumption-list`
- `inventory-ward-patient-consumption`
- `consumption-report`
- `requisition-dispatch-report`
- `transfer-report`
- `ward-inventory-reports`
- `inventory-ward-requisition-details`
- `inventory-ward-requisition-item`
- `inventory-ward-requisition-list`
- `inventory-ward-receive-stock`
- `inventory-return-form`
- `inventory-return-list`
- `inventory-ward-stock`
- `stock-reconciliation`
- `phrm-substore-requisition-add`
- `breakage-report`
- `consumption-report`
- `dispatch-report`
- `internal-consumption-report`
- `reports`
- `requisition-report`
- `stock-report`
- `transfer-report`
- `wardsupply-asset-main`
- `wardsupply-asset-new-requisition`
- `wardsupply-asset-requisition-details`
- `wardsupply-asset-requisition-list`
- `wardsupply-asset-req-dispatch-list`
- `wardsupply-asset-stock`

### Angular Services & Endpoints
- `wardsupply.bl.service.ts`
- `wardsupply.dl.service.ts`
- `wardsupply.service.ts`

