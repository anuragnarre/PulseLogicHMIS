# Fixedasset Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/FixedAsset`

### API Endpoints
- **HTTPGET** `GetAssetsDepreciationList` (Method: `GetAssetsDepreciationList`)
- **HTTPGET** `GetAssetDepreciationDetails` (Method: `GetAssetDepreciationDetails`)
- **HTTPGET** `GetAssetDepreciationMethods` (Method: `GetAssetDepreciationMethods`)
- **HTTPPOST** `PostAssetDepreciationDetails` (Method: `PostAssetDepreciationDetails`)
- **HTTPPUT** `PutAssetDepreciation` (Method: `PutAssetDepreciation`)
- **HTTPPUT** `UpdateAssetScrapDetails` (Method: `UpdateAssetScrapDetails`)
- **HTTPGET** `GetAll` (Method: `GetAll`)
- **HTTPGET** `GetVendorDetailsById` (Method: `GetVendorDetailsById`)
- **HTTPGET** `GetFaultHistory` (Method: `GetFaultHistory`)
- **HTTPGET** `GetAssetServiceHistory` (Method: `GetAssetServiceHistory`)
- **HTTPPOST** `PostAssetServiceDetails` (Method: `PostAssetServiceDetails`)
- **HTTPPUT** `PutAssetRequiredMaintenance` (Method: `PutAssetRequiredMaintenance`)
- **HTTPPUT** `PutAssetFaultResolvedDetails` (Method: `PutAssetFaultResolvedDetails`)
- **HTTPPUT** `PutRepairStatus` (Method: `PutRepairStatus`)
- **HTTPPUT** `PutAssetServiceDetails` (Method: `PutAssetServiceDetails`)
- **HTTPGET** `GetAssetContractFile` (Method: `GetAssetContractFile`)
- **HTTPPOST** `PostAssetContractFile` (Method: `PostAssetContractFile`)
- **HTTPPUT** `PutAssetContractFile` (Method: `PutAssetContractFile`)
- **HTTPPUT** `UpdateAssetDamageStatus` (Method: `UpdateAssetDamageStatus`)
- **HTTPPUT** `AssetFaultUpdate` (Method: `AssetFaultUpdate`)
- **HTTPPOST** `AssetCheckList` (Method: `AssetCheckList`)
- **HTTPPOST** `AssetFaultConfirm` (Method: `AssetFaultConfirm`)
- **HTTPPUT** `UpdateAssetMaintenanceList` (Method: `UpdateAssetMaintenanceList`)
- **HTTPGET** `AssestConditionChecklist` (Method: `AssestConditionChecklist`)
- **HTTPGET** `GetFixedAssetLocations` (Method: `GetFixedAssetLocations`)
- **HTTPGET** `GetAssetStockList` (Method: `GetAssetStockList`)
- **HTTPGET** `GetAssetsInsurance` (Method: `GetAssetsInsurance`)
- **HTTPPOST** `PostAssetInsurance` (Method: `PostAssetInsurance`)
- **HTTPPUT** `PutAssetInsurance` (Method: `PutAssetInsurance`)
- **HTTPPUT** `PutAssetDamageConfirmation` (Method: `PutAssetDamageConfirmation`)
- **HTTPPUT** `UpdateAssetManagementList` (Method: `UpdateAssetManagementList`)
- **HTTPGET** `GetAllInventoryFiscalYears` (Method: `GetAllInventoryFiscalYears`)
- **HTTPGET** `GetAllEmployeeList` (Method: `GetAllEmployeeList`)
- **HTTPGET** `GetAllDepartments` (Method: `GetAllDepartments`)
- **HTTPGET** `GetAllItems` (Method: `GetAllItems`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/fixed-asset`

### Routing / Views
- `/AssetsManagement`
- `/AssetsMaintenance`
- `/DepreciationAndDiscarding`
- `/AssetsSubstoreRequisition`
- `/AssetsSubstoreDirectDispatch`
- `/RequisitionDispatch`
- `/Reports`
- `/**`
- `/FixedAssetsMovement`

### Angular Components
- `fixed-assets-main`
- `asset-depreciation-list`
- `asset-scrap`
- `assets-maintenance-list`
- `assets-list`
- `asset-edit`
- `asset-contract-upload`
- `notify-damage`
- `fixedAssets-reports`
- `fixed-assets-movement`
- `assets-substore-requisition-dispatch-details`
- `assets-substore-requisition-dispatch`
- `assets-substore-direct-dispatch`
- `assets-substore-req-dispatch-list`

### Angular Services & Endpoints
- `fixed-asset.bl.service.ts`
- `fixed-asset.dl.service.ts`
- `fixed-asset.service.ts`

