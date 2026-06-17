using System;
using System.ComponentModel.DataAnnotations;

namespace PulseLogicHMR.ServerModel.MasterModels
{
    public class CfgPaymentGatewaySettings
    {
        [Key]
        public int GatewayId { get; set; }
        public string GatewayName { get; set; } // e.g., "Razorpay", "PayU", "PhonePe"
        public bool IsActive { get; set; }
        public string PublicKey { get; set; }   // e.g., Razorpay Key ID
        public string SecretKey { get; set; }   // e.g., Razorpay Key Secret
        public string MerchantId { get; set; }  // e.g., PayU Merchant ID
        public string WebhookSecret { get; set; }
        public string Environment { get; set; } // "Test" or "Live"
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
