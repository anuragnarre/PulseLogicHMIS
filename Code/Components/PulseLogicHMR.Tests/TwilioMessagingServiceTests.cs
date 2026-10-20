using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PulseLogicHMR.Core.Utilities.Messaging;
using Xunit;

namespace PulseLogicHMR.Tests
{
    public class TwilioMessagingServiceTests
    {
        [Fact]
        public void Constructor_MissingAccountSid_ThrowsInvalidOperationException()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Twilio:AuthToken", "test"},
                    {"Twilio:FromPhoneNumber", "test"},
                    {"Twilio:FromWhatsAppNumber", "test"}
                })
                .Build();

            Assert.Throws<InvalidOperationException>(() => new TwilioMessagingService(config));
        }

        [Fact]
        public void Constructor_ValidConfig_Succeeds()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Twilio:AccountSid", "test"},
                    {"Twilio:AuthToken", "test"},
                    {"Twilio:FromPhoneNumber", "test"},
                    {"Twilio:FromWhatsAppNumber", "test"}
                })
                .Build();

            var service = new TwilioMessagingService(config);
            Assert.NotNull(service);
        }
    }
}
