
## N+1 Query Optimization in InventoryController
**Date:** 2024-05-18
**Context:** Updating quotation items in `InventoryController.PostQuotationsDetails`.
**Issue:** The system iterated through incoming quotation items and updated them one by one. In each loop iteration, a `_inventoryDbContext.QuotationItems.Find()` call was made, followed by a `_inventoryDbContext.SaveChanges()` call. For updates with numerous items, this resulted in an N+1 query issue for the SELECT, plus an N query issue for the UPDATE/SaveChanges.
**Fix:** Refactored to fetch all relevant `QuotationItems` in a single batch using `.Where(qi => itemIds.Contains(qi.QuotationItemId)).ToDictionary(...)`. The updates were then applied to the loaded items in memory, and a single `_inventoryDbContext.SaveChanges()` was called outside the loop to commit the changes.
**Result:** Updating 1000 items in the benchmark decreased from 4340 ms to 280 ms.
