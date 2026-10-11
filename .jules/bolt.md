## ⚡ N+1 Query Fixes

### WardSupplyBL.cs - StoreStock Insertions (EF Core N+1 Issue)
**Issue:** `SaveChanges()` was being invoked within `foreach` loops for individual `StoreStock` entity inserts in `StockTransferToPharmacy` and `UpdateWardStockForConsumption`, resulting in N+1 database roundtrips.

**Fix:**
To mitigate the N+1 issue, `PHRMStockTransactionModel` generation logic was refactored. Although the constructor requires a valid (saved/generated) `StoreStockId` when a new `StoreStock` is being created, we can delay inserting `PHRMStockTransactionModel` entities using an accumulated `newTransactions` list. Only if a *new* `StoreStock` is added is a `SaveChanges()` strictly necessary during the loop. The `newTransactions` are later bulk inserted (`AddRange`) and persisted with a single `SaveChanges()` after the loop.
