using System;
using System.Linq;
using Xunit;
using PulseLogicHMR.ServerModel;

namespace PulseLogicHMR.Tests.ServerModel.PharmacyModels
{
    public class PHRMStoreModelTests
    {
        [Fact]
        public void AddPaymentMode_ValidInputs_AddsPaymentModeAndSerializes()
        {
            // Arrange
            var store = new PHRMStoreModel
            {
                Category = "dispensary"
            };

            // Act
            store.AddPaymentMode("credit", true);

            // Assert
            var modes = store.AvailablePaymentModes.ToList();
            Assert.Contains(modes, p => p.PaymentModeName == "credit");
            Assert.Contains("credit", store.AvailablePaymentModesJSON);

            var creditMode = modes.First(p => p.PaymentModeName == "credit");
            Assert.True(creditMode.IsRemarksMandatory);
        }

        [Fact]
        public void AddPaymentMode_NotDispensary_ThrowsInvalidOperationException()
        {
            // Arrange
            var store = new PHRMStoreModel
            {
                Category = "store"
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => store.AddPaymentMode("credit"));
            Assert.Equal("Payment Mode feature is only available for Disensary.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddPaymentMode_InvalidPaymentModeName_ThrowsArgumentException(string paymentModeName)
        {
            // Arrange
            var store = new PHRMStoreModel
            {
                Category = "dispensary"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => store.AddPaymentMode(paymentModeName));
            Assert.Equal("PaymentModeName", exception.ParamName);
        }
    }
}
