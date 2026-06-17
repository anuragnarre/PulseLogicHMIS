using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

            RazorpayClient client = new RazorpayClient(settings.PublicKey, settings.SecretKey);

            Dictionary<string, object> options = new Dictionary<string, object>();
            // amount should be in paise (e.g. 500 INR = 50000 paise)
            // Wait, amount param already assumed smallest unit or not? 
            // In IPaymentGatewayService I said: Amount in the smallest currency unit. 
            // So I should just pass (int)amount.
            options.Add("amount", (int)amount); 
            options.Add("currency", currency ?? "INR");
            options.Add("receipt", receiptId);
            options.Add("payment_capture", 1);

            Order order = client.Order.Create(options);

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

                Utils.verifyPaymentSignature(attributes);
                return Task.FromResult(true);
            }
            catch (SignatureVerificationError)
            {
                return Task.FromResult(false);
            }
        }
    }
}
