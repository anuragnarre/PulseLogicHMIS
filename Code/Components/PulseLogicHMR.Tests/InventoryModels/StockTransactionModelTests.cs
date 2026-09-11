using System;
using Xunit;
using PulseLogicHMR.ServerModel.InventoryModels;

namespace PulseLogicHMR.Tests.InventoryModels
{
    public class StockTransactionModelTests
    {
        [Fact]
        public void SetInQuantity_WithPositiveValue_SetsInQtyAndResetsOutQty()
        {
            // Arrange
            var model = new StockTransactionModel();

            // Note: Normally we'd set OutQty to something > 0 to prove it gets reset,
            // but OutQty has a private setter and we can only set it via SetOutQuantity.
            // Let's use SetOutQuantity first.
            model.SetOutQuantity(5.0);

            // Act
            model.SetInQuantity(10.5);

            // Assert
            Assert.Equal(10.5, model.InQty);
            Assert.Equal(0, model.OutQty);
        }

        [Fact]
        public void SetInQuantity_WithZeroValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new StockTransactionModel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => model.SetInQuantity(0));
        }

        [Fact]
        public void SetInQuantity_WithNegativeValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new StockTransactionModel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => model.SetInQuantity(-5.0));
        }
    }
}
