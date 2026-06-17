using PulseLogicHMR.Controllers.Dispensary;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ViewModel.DispensaryTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services.DispensaryTransfer
{
    public interface IDispensaryTransferService
    {
        IList<PHRMStoreModel> GetAllStoresForTransfer();
        Task<IList<GetAllDispensaryStocksVm>> GetAllDispensaryStocks(int DispensaryId);
        Task<int> TransferStock(List<StockTransferModel> value , RbacUser currentUser );
        Task<int> ReturnToStore(List<StockTransferModel> value, RbacUser currentUser);
        Task<int> DispensaryToDispensaryTransfer(List<StockTransferModel> value, RbacUser currentUser);
        Task<IList<GetAllTransactionByStoreIdDTO>> GetAllTransactionByStoreId(int storeId);
    }
}
