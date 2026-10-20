using System;
using Microsoft.Extensions.Configuration;
using Moq;
using PulseLogicHMR.Core.Utilities.Messaging;
using Xunit;

namespace PulseLogicHMR.Tests
{
    public class TwilioMessagingServiceTests
    {
        [Fact]
        public void Constructor_ShouldThrowInvalidOperationException_WhenAccountSidIsMissing()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Twilio:AccountSid"]).Returns((string)null);
            configMock.Setup(c => c["Twilio:AuthToken"]).Returns("mock-token");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => new TwilioMessagingService(configMock.Object));
            Assert.Contains("Twilio:AccountSid", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidOperationException_WhenAuthTokenIsMissing()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Twilio:AccountSid"]).Returns("mock-sid");
            configMock.Setup(c => c["Twilio:AuthToken"]).Returns((string)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => new TwilioMessagingService(configMock.Object));
            Assert.Contains("Twilio:AuthToken", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldNotThrow_WhenAccountSidAndAuthTokenArePresent()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Twilio:AccountSid"]).Returns("mock-sid");
            configMock.Setup(c => c["Twilio:AuthToken"]).Returns("mock-token");

            // Act
            var service = new TwilioMessagingService(configMock.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}
