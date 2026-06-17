# Verification Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Verification`

### API Endpoints
- **HTTPGET** `Get` (Method: `Get`)
- **HTTPGET** `~/api/Verification/GetInventoryRequisitionListBasedOnUser/{FromDate}/{ToDate}` (Method: `GetInventoryRequisitionListBasedOnUser`)
- **HTTPGET** `GetInventoryRequisitionDetails` (Method: `GetInventoryRequisitionDetails`)
- **HTTPPOST** `~/api/Verification/RejectRequisition/{RequisitionId}/{CurrentVerificationlevel}/{CurrentVerificationlevelCount}/{MaxVerificationLevel}` (Method: `RejectRequisition`)
- **HTTPGET** `~/api/Verification/GetInventoryPurchaseRequestsBasedOnUser/{FromDate}/{ToDate}` (Method: `GetInventoryPurchaseRequestsBasedOnUser`)
- **HTTPGET** `~/api/Verification/GetInventoryPurchaseRequestDetails/{PurchaseRequestId}` (Method: `GetInventoryPurchaseRequestDetails`)
- **HTTPPOST** `~/api/Verification/RejectPurchaseRequest/{PurchaseRequestId}/{CurrentVerificationlevel}/{CurrentVerificationlevelCount}/{MaxVerificationLevel}` (Method: `RejectPurchaseRequest`)
- **HTTPGET** `~/api/Verification/GetInventoryPurchaseOrdersBasedOnUser/{FromDate}/{ToDate}` (Method: `GetInventoryPurchaseOrdersBasedOnUser`)
- **HTTPGET** `~/api/Verification/GetQuotationRatesDetails/{PurchaseOrderId}` (Method: `GetQuotationRatesDetails`)
- **HTTPGET** `~/api/Verification/GetInventoryPurchaseOrderDetails/{PurchaseOrderId}` (Method: `GetInventoryPurchaseOrderDetails`)
- **HTTPPOST** `~/api/Verification/RejectPurchaseOrder/{PurchaseOrderId}/{CurrentVerificationlevel}/{CurrentVerificationlevelCount}/{MaxVerificationLevel}` (Method: `RejectPurchaseOrder`)
- **HTTPGET** `~/api/Verification/GetInventoryGRBasedOnUser/{FromDate}/{ToDate}` (Method: `GetInventoryGRBasedOnUser`)
- **HTTPGET** `~/api/Verification/GetInventoryGRDetails/{GoodsReceiptId}` (Method: `GetInventoryGRDetails`)
- **HTTPPOST** `~/api/Verification/RejectGoodsReceipt/{GoodsReceiptId}/{CurrentVerificationlevel}/{CurrentVerificationlevelCount}/{MaxVerificationLevel}` (Method: `RejectGoodsReceipt`)
- **HTTPGET** `PharmacyPurchaseOrdersBasedOnUser` (Method: `GetPharmacyPurchaseOrdersBasedOnUser`)
- **HTTPGET** `PharmacyPurchaseOrderInfo` (Method: `GetPharmacyPurchaseOrderInfo`)
- **HTTPPOST** `ApprovePharmacyPurchaseOrder` (Method: `ApprovePharmacyPurchaseOrder`)
- **HTTPPOST** `RejectPharmacyPurchaseOrder` (Method: `RejectPharmacyPurchaseOrder`)
- **HTTPGET** `PharmacyRequisitionsBasedOnUser` (Method: `GetPharmacyRequisitionsBasedOnUser`)
- **HTTPGET** `PharmacyRequisitionInfo` (Method: `GetPharmacyRequisitionInfo`)
- **HTTPPOST** `ApprovePharmacyRequisition` (Method: `ApprovePharmacyRequisition`)
- **HTTPPOST** `RejectPharmacyRequisition` (Method: `RejectPharmacyRequisition`)

### Services
Backend services encapsulate business logic and data access for Verification.
- `IVerificationService.cs`
- `VerificationService.cs`
- `Verifier_DTO.cs`

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/verification`

### Routing / Views
- `/Inventory`
- `/Requisition`
- `/RequisitionDetails`
- `/**`
- `/PurchaseRequest`
- `/PurchaseRequestDetail`
- `/PurchaseOrder`
- `/PurchaseOrderVerify`
- `/GoodsReceipt`
- `/GoodsReceiptVerify`
- `/Pharmacy`

### Angular Components
- `verification-inventory`
- `goods-receipt-list`
- `goods-receipt-verify`
- `purchase-order-list`
- `purchase-order-verify`
- `purchase-request-detail`
- `purchase-request-list`
- `inventory-requisition-details`
- `inventory-requisition-list`
- `pharmacy-verification`
- `pharmacy-verification-purchase-order-list`
- `pharmacy-verification-purchase-order`
- `pharmacy-verification-requisition-list`
- `pharmacy-verification-requisition`
- `verification-main`

### Angular Services & Endpoints
- `verification.bl.service.ts`
- `verification.dl.service.ts`
- `verification.service.ts`

