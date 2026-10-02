using System;
using Xunit;
using PulseLogicHMR.ServerModel.InventoryModels;

namespace PulseLogicHMR.Tests.InventoryModels
{
    public class StoreStockModelTests
    {
        [Fact]
        public void ConfirmStockReceived_ShouldDecreaseUnConfirmedQtyInAndIncreaseAvailableQuantity()
        {
            // Arrange
            var stockMaster = new StockMasterModel(itemId: 1, batchNo: "B1", expiryDate: null, costPrice: 10m, mRP: 15m, specification: "Spec", createdBy: 1, createdOn: DateTime.Now);
            // using reflection to set StockId since it's private set and we need it to instantiate StoreStockModel properly
            typeof(StockMasterModel).GetProperty("StockId")?.SetValue(stockMaster, 1);

            var storeStock = new StoreStockModel(stockMaster, storeId: 1, quantity: 10, transactionType: "IN", transactionDate: DateTime.Now, currentDate: DateTime.Now, referenceNo: 1, createdBy: 1, fiscalYearId: 1, needConfirmation: true);

            var initialUnconfirmedQtyIn = storeStock.UnConfirmedQty_In; // should be 10
            var initialAvailableQuantity = storeStock.AvailableQuantity; // should be 0

            // Act
            storeStock.ConfirmStockReceived(5);

            // Assert
            Assert.Equal(initialUnconfirmedQtyIn - 5, storeStock.UnConfirmedQty_In);
            Assert.Equal(initialAvailableQuantity + 5, storeStock.AvailableQuantity);
        }

        [Fact]
        public void ConfirmStockReceived_ShouldHandleZeroQuantity()
        {
            // Arrange
            var stockMaster = new StockMasterModel(itemId: 1, batchNo: "B1", expiryDate: null, costPrice: 10m, mRP: 15m, specification: "Spec", createdBy: 1, createdOn: DateTime.Now);
            typeof(StockMasterModel).GetProperty("StockId")?.SetValue(stockMaster, 1);

            var storeStock = new StoreStockModel(stockMaster, storeId: 1, quantity: 10, transactionType: "IN", transactionDate: DateTime.Now, currentDate: DateTime.Now, referenceNo: 1, createdBy: 1, fiscalYearId: 1, needConfirmation: true);

            var initialUnconfirmedQtyIn = storeStock.UnConfirmedQty_In;
            var initialAvailableQuantity = storeStock.AvailableQuantity;

            // Act
            storeStock.ConfirmStockReceived(0);

            // Assert
            Assert.Equal(initialUnconfirmedQtyIn, storeStock.UnConfirmedQty_In);
            Assert.Equal(initialAvailableQuantity, storeStock.AvailableQuantity);
        }

        [Fact]
        public void ConfirmStockReceived_ShouldHandleNegativeQuantity()
        {
            // Arrange
            var stockMaster = new StockMasterModel(itemId: 1, batchNo: "B1", expiryDate: null, costPrice: 10m, mRP: 15m, specification: "Spec", createdBy: 1, createdOn: DateTime.Now);
            typeof(StockMasterModel).GetProperty("StockId")?.SetValue(stockMaster, 1);

            var storeStock = new StoreStockModel(stockMaster, storeId: 1, quantity: 10, transactionType: "IN", transactionDate: DateTime.Now, currentDate: DateTime.Now, referenceNo: 1, createdBy: 1, fiscalYearId: 1, needConfirmation: true);

            var initialUnconfirmedQtyIn = storeStock.UnConfirmedQty_In;
            var initialAvailableQuantity = storeStock.AvailableQuantity;

            // Act
            storeStock.ConfirmStockReceived(-5);

            // Assert
            Assert.Equal(initialUnconfirmedQtyIn - (-5), storeStock.UnConfirmedQty_In);
            Assert.Equal(initialAvailableQuantity + (-5), storeStock.AvailableQuantity);
        }
    }
}
