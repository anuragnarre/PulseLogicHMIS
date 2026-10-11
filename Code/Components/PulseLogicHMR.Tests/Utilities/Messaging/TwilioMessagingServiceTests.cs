using System.Threading.Tasks;
using Xunit;
using PulseLogicHMR.Core.Utilities.Messaging;

namespace PulseLogicHMR.Tests.Utilities.Messaging
{
    public class TwilioMessagingServiceTests
    {
        [Fact]
        public async Task SendSmsAsync_ShouldReturnTrue()
        {
            // Arrange
            var service = new TwilioMessagingService();
            var phoneNumber = "+1234567890";
            var message = "Test SMS Message";

            // Act
            var result = await service.SendSmsAsync(phoneNumber, message);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendWhatsAppAsync_ShouldReturnTrue()
        {
            // Arrange
            var service = new TwilioMessagingService();
            var phoneNumber = "+1234567890";
            var message = "Test WhatsApp Message";

            // Act
            var result = await service.SendWhatsAppAsync(phoneNumber, message);

            // Assert
            Assert.True(result);
        }
    }
}
