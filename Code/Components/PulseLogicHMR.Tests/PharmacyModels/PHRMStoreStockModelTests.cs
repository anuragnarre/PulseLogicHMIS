using System;
using PulseLogicHMR.ServerModel;
using Xunit;

namespace PulseLogicHMR.Tests.PharmacyModels
{
    public class PHRMStoreStockModelTests
    {
        [Fact]
        public void IncreaseUnconfirmedQty_WithPositiveValues_UpdatesQuantitiesCorrectly()
        {
            // Arrange
            var model = new PHRMStoreStockModel();

            // Act
            model.IncreaseUnconfirmedQty(10.5, 5.2);

            // Assert
            Assert.Equal(10.5, model.UnConfirmedQty_In, 4); // Use precision to avoid floating-point comparison issues
            Assert.Equal(5.2, model.UnConfirmedQty_Out, 4);

            // Act again to test accumulation
            model.IncreaseUnconfirmedQty(2.0, 1.1);

            // Assert
            Assert.Equal(12.5, model.UnConfirmedQty_In, 4);
            Assert.Equal(6.3, model.UnConfirmedQty_Out, 4);
        }

        [Fact]
        public void IncreaseUnconfirmedQty_WithNegativeInQty_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new PHRMStoreStockModel();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => model.IncreaseUnconfirmedQty(-1.0, 5.0));
            Assert.Equal("Can not insert negative value", ex.Message);
        }

        [Fact]
        public void IncreaseUnconfirmedQty_WithNegativeOutQty_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new PHRMStoreStockModel();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => model.IncreaseUnconfirmedQty(5.0, -1.0));
            Assert.Equal("Can not insert negative value", ex.Message);
        }

        [Fact]
        public void IncreaseUnconfirmedQty_WithZeroValues_DoesNotChangeQuantities()
        {
            // Arrange
            var model = new PHRMStoreStockModel();

            // Set initial state
            model.IncreaseUnconfirmedQty(10.0, 5.0);

            // Act
            model.IncreaseUnconfirmedQty(0, 0);

            // Assert
            Assert.Equal(10.0, model.UnConfirmedQty_In);
            Assert.Equal(5.0, model.UnConfirmedQty_Out);
        }
    }
}
