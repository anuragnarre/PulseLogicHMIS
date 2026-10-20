using System;
using Xunit;
using PulseLogicHMR.ServerModel.InventoryModels;

namespace PulseLogicHMR.Tests.InventoryModels
{
    public class StoreStockModelTests
    {
        [Fact]
        public void DecreaseStock_QuantityZero_ThrowsInvalidOperationException()
        {
            // Arrange
            var sut = new StoreStockModel();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                sut.DecreaseStock(0, "TestTransaction", null, DateTime.Now, null, 1, 1, true));

            Assert.Equal("Cannot decrease 0 stock. Please Check.", ex.Message);
        }

        [Fact]
        public void DecreaseStock_QuantityNegative_ThrowsInvalidOperationException()
        {
            // Arrange
            var sut = new StoreStockModel();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                sut.DecreaseStock(-5.5, "TestTransaction", null, DateTime.Now, null, 1, 1, true));

            Assert.Equal("Cannot decrease 0 stock. Please Check.", ex.Message);
        }
    }
}
