
## Optimization: N+1 Query on Accounting Transactions
- **Type**: N+1 Query / SaveChanges Loop
- **Description**: Replaced per-transaction `SaveChanges()` inside `.ForEach` loops across 4 branches in `AccountingTransferData.cs` with a single batch `SaveChanges()` after the loop.
- **Impact**: Reduced database round trips from N to 1 for `Transaction` and `AccountingTransactionHistory` records, saving ~5.7s for 1000 transactions in benchmark.
