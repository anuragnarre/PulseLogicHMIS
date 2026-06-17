using System.Threading.Tasks;

namespace PulseLogicHMR.Services
{
    public interface INepaliReceiptService
    {
        Task<DonationGRVm> GetDonationGRView(int GoodsReceiptId);
        NepaliRequisitionVm GetNepaliRequisitionView(int RequisitionId, string ModuleType);
        NepaliDispatchVm GetNepaliDispatchView(int DispatchId, int RequisitionId, string ModuleType);
    }
}
