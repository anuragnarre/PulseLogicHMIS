## Performance Issue: N+1 inside PulseLogicHMR.AccTransfer
The `PostTxnData` method in `AccountingTransferData.cs` contains loops parsing transactions. Inside these loops, `AccountingTransactionHistoryModel`s were being added individually, followed by a `SaveChanges()` call per iteration, leading to massive N+1 database queries.
**Solution**: Deferred the commit by maintaining a list (`List<AccountingTransactionHistoryModel>`), and then saving outside the loop in a batch operation using `.AddRange()`.
