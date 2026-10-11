using System.Threading.Tasks;
using PulseLogicHMR.Core.Billing.PaymentGateway;
using PulseLogicHMR.ServerModel.MasterModels;
using Xunit;
using Razorpay.Api;

namespace PulseLogicHMR.Tests.Core.Billing.PaymentGateway
{
    public class RazorpayPaymentServiceTests
    {
        [Fact]
        public async Task VerifyPaymentAsync_InvalidSignature_ReturnsFalse()
        {
            var service = new RazorpayPaymentService();

            // Note: VerifyPaymentAsync internally calls Utils.verifyPaymentSignature which relies on
            // RazorpayClient.Secret being set. In a real application, RazorpayClient is instantiated
            // elsewhere (e.g. during CreateOrderAsync) which sets this static property.
            // For testing the verification in isolation, we must instantiate the client here to set the secret.
            var client = new RazorpayClient("test_pub", "test_sec");

            var settings = new CfgPaymentGatewaySettings { PublicKey = "test_pub", SecretKey = "test_sec" };

            // "invalid_sig" will definitely not match the expected HMAC SHA256 signature
            var result = await service.VerifyPaymentAsync("order_123", "pay_123", "invalid_sig", settings);

            Assert.False(result);
        }

        [Fact]
        public async Task VerifyPaymentAsync_EmptyParameters_ReturnsFalse()
        {
            var service = new RazorpayPaymentService();
            var settings = new CfgPaymentGatewaySettings { PublicKey = "test_pub", SecretKey = "test_sec" };

            var result1 = await service.VerifyPaymentAsync("", "pay_123", "sig", settings);
            var result2 = await service.VerifyPaymentAsync("order_123", "", "sig", settings);
            var result3 = await service.VerifyPaymentAsync("order_123", "pay_123", "", settings);
            var result4 = await service.VerifyPaymentAsync(null, null, null, settings);

            Assert.False(result1);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);
        }

        [Fact]
        public async Task VerifyPaymentAsync_ValidSignature_ReturnsTrue()
        {
            var service = new RazorpayPaymentService();

            string secret = "test_sec";
            string orderId = "order_123";
            string paymentId = "pay_123";

            var client = new RazorpayClient("test_pub", secret);
            var settings = new CfgPaymentGatewaySettings { PublicKey = "test_pub", SecretKey = secret };

            // Generate a valid signature for testing
            string payload = orderId + "|" + paymentId;
            string validSignature = GetExpectedSignature(payload, secret);

            var result = await service.VerifyPaymentAsync(orderId, paymentId, validSignature, settings);

            Assert.True(result);
        }

        private string GetExpectedSignature(string payload, string secret)
        {
            byte[] secretBytes = System.Text.Encoding.UTF8.GetBytes(secret);
            byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

            using (var hmac = new System.Security.Cryptography.HMACSHA256(secretBytes))
            {
                byte[] hashMessage = hmac.ComputeHash(payloadBytes);
                return System.BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
    }
}
