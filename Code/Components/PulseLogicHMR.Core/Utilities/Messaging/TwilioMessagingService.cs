using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PulseLogicHMR.Core.Utilities.Messaging
{
    public class TwilioMessagingService : IMessagingService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;
        private readonly string _fromWhatsAppNumber;

        public TwilioMessagingService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"] ?? throw new InvalidOperationException("Missing configuration for Twilio:AccountSid");
            _authToken = configuration["Twilio:AuthToken"] ?? throw new InvalidOperationException("Missing configuration for Twilio:AuthToken");
            _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"] ?? throw new InvalidOperationException("Missing configuration for Twilio:FromPhoneNumber");
            _fromWhatsAppNumber = configuration["Twilio:FromWhatsAppNumber"] ?? throw new InvalidOperationException("Missing configuration for Twilio:FromWhatsAppNumber");
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            // Stub for SMS sending logic
            Console.WriteLine($"[SMS] To: {phoneNumber}, Message: {message}");
            return await Task.FromResult(true);
        }

        public async Task<bool> SendWhatsAppAsync(string phoneNumber, string message)
        {
            // Stub for WhatsApp sending logic using Twilio API
            string formattedToNumber = phoneNumber.StartsWith("whatsapp:") ? phoneNumber : $"whatsapp:{phoneNumber}";
            Console.WriteLine($"[WhatsApp] To: {formattedToNumber}, Message: {message}");
            return await Task.FromResult(true);
        }
    }
}
