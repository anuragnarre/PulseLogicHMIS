using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PulseLogicHMR.ServerModel.MasterModels;
using Razorpay.Api;
using Razorpay.Api.Errors;

namespace PulseLogicHMR.Core.Billing.PaymentGateway
{
    public class RazorpayPaymentService : IPaymentGatewayService
    {
        public Task<string> CreateOrderAsync(decimal amount, string currency, string receiptId, CfgPaymentGatewaySettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.PublicKey) || string.IsNullOrEmpty(settings.SecretKey))
            {
                throw new ArgumentException("Invalid Razorpay settings.");
            }

            var client = CreateClient(settings.PublicKey, settings.SecretKey);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", (int)amount); 
            options.Add("currency", currency ?? "INR");
            options.Add("receipt", receiptId);
            options.Add("payment_capture", 1);

            Order order = CreateOrder(client, options);

            return Task.FromResult(order["id"].ToString());
        }

        public Task<bool> VerifyPaymentAsync(string orderId, string paymentId, string signature, CfgPaymentGatewaySettings settings)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(paymentId))
            {
                return Task.FromResult(false);
            }

            try
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_order_id", orderId },
                    { "razorpay_payment_id", paymentId },
                    { "razorpay_signature", signature }
                };

                VerifySignature(attributes);
                return Task.FromResult(true);
            }
            catch (SignatureVerificationError)
            {
                return Task.FromResult(false);
            }
        }

        protected virtual RazorpayClient CreateClient(string key, string secret)
        {
            return new RazorpayClient(key, secret);
        }

        protected virtual Order CreateOrder(RazorpayClient client, Dictionary<string, object> options)
        {
            return client.Order.Create(options);
        }

        protected virtual void VerifySignature(Dictionary<string, string> attributes)
        {
            Utils.verifyPaymentSignature(attributes);
        }
    }
}
