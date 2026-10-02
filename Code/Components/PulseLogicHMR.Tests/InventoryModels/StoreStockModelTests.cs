using System;
using System.Linq;
using System.Reflection;
using Xunit;
using PulseLogicHMR.ServerModel.InventoryModels;

namespace PulseLogicHMR.Tests.InventoryModels
{
    public class StoreStockModelTests
    {
        [Fact]
        public void AddStock_WhenQuantityIsZeroOrLess_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new StoreStockModel();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                model.AddStock(0, "Test", DateTime.Now, DateTime.Now, null, 1, 1, true));
            Assert.Equal("Cannot add 0 stock. Please Check.", ex.Message);

            ex = Assert.Throws<InvalidOperationException>(() =>
                model.AddStock(-1, "Test", DateTime.Now, DateTime.Now, null, 1, 1, true));
            Assert.Equal("Cannot add 0 stock. Please Check.", ex.Message);
        }

        [Fact]
        public void AddStock_WhenNeedConfirmationIsTrue_AddsToUnConfirmedQtyIn()
        {
            // Arrange
            var model = new StoreStockModel();
            var stockMaster = new StockMasterModel();
            model.StockMaster = stockMaster; // Required by StockTransactionModel constructor

            var initialUnconfirmed = model.UnConfirmedQty_In;
            var initialAvailable = model.AvailableQuantity;

            // Act
            model.AddStock(10, "Test", DateTime.Now, DateTime.Now, null, 1, 1, true);

            // Assert
            Assert.Equal(initialUnconfirmed + 10, model.UnConfirmedQty_In);
            Assert.Equal(initialAvailable, model.AvailableQuantity);
        }

        [Fact]
        public void AddStock_WhenNeedConfirmationIsFalse_AddsToAvailableQuantity()
        {
            // Arrange
            var model = new StoreStockModel();
            var stockMaster = new StockMasterModel();
            model.StockMaster = stockMaster; // Required by StockTransactionModel constructor

            var initialUnconfirmed = model.UnConfirmedQty_In;
            var initialAvailable = model.AvailableQuantity;

            // Act
            model.AddStock(15, "Test", DateTime.Now, DateTime.Now, null, 1, 1, false);

            // Assert
            Assert.Equal(initialUnconfirmed, model.UnConfirmedQty_In);
            Assert.Equal(initialAvailable + 15, model.AvailableQuantity);
        }

        [Fact]
        public void AddStock_WhenTransactionDateIsNull_UsesCurrentDate()
        {
            // Arrange
            var model = new StoreStockModel();
            SetProperty(model, "StoreId", 1);
            SetProperty(model, "StockId", 2);
            SetProperty(model, "ItemId", 3);

            var currentDate = new DateTime(2023, 1, 1, 12, 0, 0);

            var stockMaster = new StockMasterModel();
            model.StockMaster = stockMaster; // Required by StockTransactionModel constructor

            // Act
            model.AddStock(5, "Test", null, currentDate, null, 1, 1, false);

            // Assert
            var transaction = model.StockTransactions.Single();
            Assert.Equal(currentDate, transaction.TransactionDate);
        }

        [Fact]
        public void AddStock_CreatesStockTransactionWithCorrectValues()
        {
            // Arrange
            var model = new StoreStockModel();
            SetProperty(model, "StoreId", 100);
            SetProperty(model, "StockId", 200);
            SetProperty(model, "ItemId", 300);

            // We need a StockMaster with properties for StockTransactionModel
            var stockMaster = new StockMasterModel(300, "BATCH123", new DateTime(2025, 1, 1), 100m, 120m, "Spec", 1, null);
            model.StockMaster = stockMaster;

            var transactionDate = new DateTime(2023, 1, 15);
            var currentDate = new DateTime(2023, 1, 16);
            int? referenceNo = 12345;
            int createdBy = 99;
            int fiscalYearId = 5;

            // Act
            model.AddStock(25, "Purchase", transactionDate, currentDate, referenceNo, createdBy, fiscalYearId, false);

            // Assert
            Assert.Single(model.StockTransactions);
            var transaction = model.StockTransactions.Single();

            Assert.Equal("Purchase", transaction.TransactionType);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(referenceNo, transaction.ReferenceNo);
            Assert.Equal(createdBy, transaction.CreatedBy);
            Assert.Equal(currentDate, transaction.CreatedOn);
            Assert.Equal(fiscalYearId, transaction.FiscalYearId);
            Assert.Equal(25, transaction.InQty);
            Assert.Equal(0, transaction.OutQty);

            // The StockTransaction constructor does not seem to set StoreStock property on itself directly.
            // Asserting the relation might be dependent on EF core's relation fixup when saving if not explicitly set.
            // But we should check it doesn't fail. Let's just check other props are correctly set.
        }

        private void SetProperty(object obj, string propertyName, object value)
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value);
            }
        }
    }
}
