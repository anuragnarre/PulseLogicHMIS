using System.Collections.Generic;

namespace PulseLogicHMR.ServerModel.BillingModels.DischargeModel
{
    public class PendingBill
    {
        public IpBillingTxnVM BillingPendingItems { get; set; }
        public List<PharmacyPendingBillItem> PharmacyPendingItem { get; set; }
        public decimal PharmacyTotalAmount { get; set; }
    }
}
