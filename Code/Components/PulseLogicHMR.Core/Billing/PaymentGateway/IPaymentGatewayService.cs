using System.Threading.Tasks;
using PulseLogicHMR.ServerModel.MasterModels;

namespace PulseLogicHMR.Core.Billing.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        /// <summary>
        /// Creates an order or payment intent with the gateway.
        /// </summary>
        /// <param name="amount">Amount in the smallest currency unit (e.g., paisa for INR)</param>
        /// <param name="currency">Currency code (e.g., "INR")</param>
        /// <param name="receiptId">Unique receipt or transaction identifier from our system</param>
        /// <param name="settings">The gateway settings to use</param>
        /// <returns>A gateway-specific order identifier or payment URL</returns>
        Task<string> CreateOrderAsync(decimal amount, string currency, string receiptId, CfgPaymentGatewaySettings settings);

        /// <summary>
        /// Verifies a payment signature/status returned by the gateway.
        /// </summary>
        /// <param name="orderId">The gateway order identifier</param>
        /// <param name="paymentId">The gateway payment identifier</param>
        /// <param name="signature">The signature or token provided by the gateway</param>
        /// <param name="settings">The gateway settings to use</param>
        /// <returns>True if verification succeeds, otherwise false.</returns>
        Task<bool> VerifyPaymentAsync(string orderId, string paymentId, string signature, CfgPaymentGatewaySettings settings);
    }
}
