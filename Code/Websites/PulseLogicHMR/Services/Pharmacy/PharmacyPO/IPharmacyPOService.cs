using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ViewModel.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services.Pharmacy.PharmacyPO
{
    public interface IPharmacyPOService
    {
        Task<GetPharmacyPOEditVm> GetPurchaseOrderForEdit(int id);
        Task<GetItemsForPOViewModel> GetAllAsync();
        int UpdatePurchaseOrder(PHRMPurchaseOrderModel value, RbacUser currentUser);
    }
}
