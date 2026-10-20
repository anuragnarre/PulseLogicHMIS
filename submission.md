💡 **What:** Batched database queries when updating stock on `ReceiveIncomingStockAsync`.
🎯 **Why:** To resolve an N+1 query issue inside nested loops for `StockTransactions` and `StoreStocks`.
📊 **Measured Improvement:** Baseline performance measurement showed an execution time of 1155 ms for a batch of 100 dispatch items. After refactoring to batch the queries and move `SaveChangesAsync` outside the loop, the execution time was reduced to 253-271 ms, an improvement of roughly 4.5x.
