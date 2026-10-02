using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PulseLogicHMR.Enums;

namespace PulseLogicHMR.ViewModel.Pharmacy
{
    #region Output ViewModel
    public class ReceiveIncomingStockViewModel
    {
    }
    #endregion

    #region Methods
    public static class ReceiveIncomingStockFunc
    {
        public static async Task<int> ReceiveIncomingStockAsync(this PharmacyDbContext db, int DispatchId, string ReceivingRemarks, RbacUser currentUser)
        {
            using (var dbResource = db.Database.BeginTransaction())
            {
                try
                {
                    var currentDate = DateTime.Now;
                    List<PHRMDispatchItemsModel> dispatchItemsToUpdate = await db.StoreDispatchItems.Where(itm => itm.DispatchId == DispatchId).ToListAsync();
                    if (dispatchItemsToUpdate == null || dispatchItemsToUpdate.Count == 0) { throw new Exception("Items Not Found."); };

                    IReadOnlyList<string> IncomingTxns = new List<string>() { ENUM_PHRM_StockTransactionType.TransferItem, ENUM_PHRM_StockTransactionType.DispatchedItem };

                    var dispatchItemIds = dispatchItemsToUpdate.Select(di => di.DispatchItemsId).ToList();

                    // Pre-fetch all related stock transactions
                    var allStockTxns = await db.StockTransactions
                        .Where(ST => dispatchItemIds.Contains(ST.ReferenceNo) && IncomingTxns.Contains(ST.TransactionType) && ST.IsActive == true)
                        .ToListAsync();

                    // Note: StoreStockId is nullable int in model but we only want non-nulls or can cast.
                    var storeStockIds = allStockTxns.Select(st => st.StoreStockId).Distinct().ToList();

                    // Pre-fetch all related store stocks
                    // s.StoreStockId is int? in the model, so we need to handle it or cast it
                    var allStoreStocks = await db.StoreStocks
                        .Where(s => s.StoreStockId.HasValue && storeStockIds.Contains(s.StoreStockId.Value) && s.IsActive == true)
                        .ToDictionaryAsync(s => s.StoreStockId.Value);

                    var stockTxnsByDispatchItemId = allStockTxns
                        .GroupBy(st => st.ReferenceNo)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var dispatchedItem in dispatchItemsToUpdate)
                    {
                        if (stockTxnsByDispatchItemId.TryGetValue(dispatchedItem.DispatchItemsId, out var StockTxns))
                        {
                            foreach (var stockTxn in StockTxns)
                            {
                                if (allStoreStocks.TryGetValue(stockTxn.StoreStockId, out var stock))
                                {
                                    if (stock.StoreId == dispatchedItem.SourceStoreId)
                                    {
                                        // Decrease Unconfirmed Qty out
                                        stock.DecreaseUnconfirmedQty(0, stockTxn.OutQty);
                                    }
                                    else
                                    {
                                        // Decrease UnconfirmedQtyIn
                                        stock.DecreaseUnconfirmedQty(stockTxn.InQty, 0);
                                        // Increate Available Qty
                                        stock.UpdateAvailableQuantity(stock.AvailableQuantity + stockTxn.InQty);
                                    }
                                }
                            }
                        }

                        // Update the Received Status in Dispatched Items Row in Dispatch Table
                        dispatchedItem.ReceivedById = currentUser.EmployeeId;
                        dispatchedItem.ReceivedOn = currentDate;
                        dispatchedItem.ReceivedRemarks = ReceivingRemarks;
                    }

                    await db.SaveChangesAsync();
                    dbResource.Commit();
                }
                catch (Exception)
                {
                    dbResource.Rollback();
                    throw;
                }
            }
            return DispatchId;
        }
    }
    #endregion
}
