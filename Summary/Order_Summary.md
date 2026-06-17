# Order Module Summary

## Backend Implementation
**Directory**: `Code/Websites/PulseLogicHMR/Controllers/Order`

### API Endpoints
- **HTTPGET** `ActiveOrders` (Method: `ActiveOrders`)
- **HTTPGET** `OrderItems` (Method: `OrderItems`)
- **HTTPGET** `GenericMaps` (Method: `GenericMaps`)
- **HTTPGET** `BillingItems` (Method: `BillingItems`)
- **HTTPGET** `EmployeePreferences` (Method: `EmployeePreferences`)
- **HTTPPOST** `EmployeePreference` (Method: `PostEmployeePreference`)
- **HTTPPUT** `EmployeePreference` (Method: `PutEmployeePreference`)

---

## Frontend Implementation
**Directory**: `wwwroot/PulseLogicApp/src/app/orders`

### Routing / Views
- `/OrderRequisition`
- `/PrintMedication`
- `/**`
- `/lab-select`
- `/OrderLabTests`
- `/imaging-select`

### Angular Components
- `orders-main`
- `order-requisition`
- `order-select`

### Angular Services & Endpoints
- `order.service.ts`
- `orders.bl.service.ts`
- `orders.dl.service.ts`

