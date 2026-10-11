using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PulseLogicHMR.Core.Billing.PaymentGateway;
using PulseLogicHMR.ServerModel.MasterModels;
using Razorpay.Api;
using Razorpay.Api.Errors;
using Xunit;

namespace PulseLogicHMR.Tests.Billing.PaymentGateway
{
    public class TestableRazorpayPaymentService : RazorpayPaymentService
    {
        public Func<string, string, RazorpayClient>? MockCreateClient { get; set; }
        public Func<RazorpayClient?, Dictionary<string, object>, Order>? MockCreateOrder { get; set; }
        public Action<Dictionary<string, string>>? MockVerifySignature { get; set; }
        public int VerifySignatureCallCount { get; private set; }

        protected override RazorpayClient CreateClient(string key, string secret)
        {
            if (MockCreateClient != null) return MockCreateClient(key, secret);
            return null!; // Return null intentionally if not mocked, since mock order bypasses client usage
        }

        protected override Order CreateOrder(RazorpayClient client, Dictionary<string, object> options)
        {
            if (MockCreateOrder != null) return MockCreateOrder(client, options);
            return base.CreateOrder(client, options);
        }

        protected override void VerifySignature(Dictionary<string, string> attributes)
        {
            VerifySignatureCallCount++;
            if (MockVerifySignature != null)
            {
                MockVerifySignature(attributes);
            }
        }
    }

    public class RazorpayPaymentServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_ThrowsArgumentException_WhenSettingsAreNull()
        {
            var service = new RazorpayPaymentService();
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOrderAsync(100, "INR", "receipt1", null!));
            Assert.Equal("Invalid Razorpay settings.", exception.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsArgumentException_WhenPublicKeyMissing()
        {
            var service = new RazorpayPaymentService();
            var settings = new CfgPaymentGatewaySettings { PublicKey = null!, SecretKey = "secret" };
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOrderAsync(100, "INR", "receipt1", settings));
            Assert.Equal("Invalid Razorpay settings.", exception.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsArgumentException_WhenSecretKeyMissing()
        {
            var service = new RazorpayPaymentService();
            var settings = new CfgPaymentGatewaySettings { PublicKey = "pub", SecretKey = null! };
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOrderAsync(100, "INR", "receipt1", settings));
            Assert.Equal("Invalid Razorpay settings.", exception.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_ReturnsOrderId_WhenSuccessful()
        {
            var expectedOrderId = "order_123456789";

            var mockOrder = new Order();
            var field = typeof(Entity).GetField("Attributes", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var attributes = JObject.Parse($"{{\"id\":\"{expectedOrderId}\"}}");
            field?.SetValue(mockOrder, attributes);

            var service = new TestableRazorpayPaymentService();

            service.MockCreateClient = (key, secret) =>
            {
                Assert.Equal("test_pub", key);
                Assert.Equal("test_sec", secret);
                return null!;
            };

            service.MockCreateOrder = (client, options) =>
            {
                Assert.Equal(100, options["amount"]);
                Assert.Equal("INR", options["currency"]);
                Assert.Equal("receipt1", options["receipt"]);
                Assert.Equal(1, options["payment_capture"]);
                return mockOrder;
            };

            var settings = new CfgPaymentGatewaySettings { PublicKey = "test_pub", SecretKey = "test_sec" };
            var orderId = await service.CreateOrderAsync(100, "INR", "receipt1", settings);

            Assert.Equal(expectedOrderId, orderId);
        }

        [Theory]
        [InlineData(null, "pay_123", "sig_123")]
        [InlineData("order_123", null, "sig_123")]
        [InlineData("order_123", "pay_123", null)]
        [InlineData("", "pay_123", "sig_123")]
        [InlineData("order_123", "", "sig_123")]
        [InlineData("order_123", "pay_123", "")]
        public async Task VerifyPaymentAsync_ReturnsFalse_WhenRequiredParametersAreMissing(string? orderId, string? paymentId, string? signature)
        {
            var service = new TestableRazorpayPaymentService();
            var settings = new CfgPaymentGatewaySettings { PublicKey = "pub", SecretKey = "sec" };

            var result = await service.VerifyPaymentAsync(orderId!, paymentId!, signature!, settings);

            Assert.False(result);
            Assert.Equal(0, service.VerifySignatureCallCount);
        }

        [Fact]
        public async Task VerifyPaymentAsync_ReturnsTrue_WhenSignatureIsVerified()
        {
            var orderId = "order_123";
            var paymentId = "pay_123";
            var signature = "sig_123";
            var settings = new CfgPaymentGatewaySettings { PublicKey = "pub", SecretKey = "sec" };

            var service = new TestableRazorpayPaymentService();
            service.MockVerifySignature = (attrs) =>
            {
                Assert.Equal(orderId, attrs["razorpay_order_id"]);
                Assert.Equal(paymentId, attrs["razorpay_payment_id"]);
                Assert.Equal(signature, attrs["razorpay_signature"]);
            };

            var result = await service.VerifyPaymentAsync(orderId, paymentId, signature, settings);

            Assert.True(result);
            Assert.Equal(1, service.VerifySignatureCallCount);
        }

        [Fact]
        public async Task VerifyPaymentAsync_ReturnsFalse_WhenSignatureVerificationFails()
        {
            var orderId = "order_123";
            var paymentId = "pay_123";
            var signature = "sig_invalid";
            var settings = new CfgPaymentGatewaySettings { PublicKey = "pub", SecretKey = "sec" };

            var service = new TestableRazorpayPaymentService();
            service.MockVerifySignature = (attrs) =>
            {
                throw new SignatureVerificationError("Invalid signature");
            };

            var result = await service.VerifyPaymentAsync(orderId, paymentId, signature, settings);

            Assert.False(result);
            Assert.Equal(1, service.VerifySignatureCallCount);
        }
    }
}
