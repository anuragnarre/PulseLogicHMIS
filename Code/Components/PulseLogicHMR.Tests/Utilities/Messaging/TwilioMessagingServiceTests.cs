using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using PulseLogicHMR.Core.Utilities.Messaging;

namespace PulseLogicHMR.Tests.Utilities.Messaging
{
    public class TwilioMessagingServiceTests : IDisposable
    {
        private readonly StringWriter _consoleOutput;
        private readonly TextWriter _originalOutput;

        public TwilioMessagingServiceTests()
        {
            _consoleOutput = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _consoleOutput.Dispose();
        }

        [Theory]
        [InlineData("+1234567890", "whatsapp:+1234567890")]
        [InlineData("whatsapp:+0987654321", "whatsapp:+0987654321")]
        [InlineData("12345", "whatsapp:12345")]
        public async Task SendWhatsAppAsync_FormatsPhoneNumberCorrectly(string inputNumber, string expectedFormattedNumber)
        {
            // Arrange
            var service = new TwilioMessagingService();
            string message = "Test Message";

            // Act
            var result = await service.SendWhatsAppAsync(inputNumber, message);

            // Assert
            Assert.True(result);
            string output = _consoleOutput.ToString();
            string expectedSubstring = $"[WhatsApp] To: {expectedFormattedNumber}, Message: {message}";
            Assert.Contains(expectedSubstring, output);
        }
    }
}
