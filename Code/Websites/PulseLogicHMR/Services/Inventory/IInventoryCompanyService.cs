using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ViewModel.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services
{
    public interface IInventoryCompanyService
    {
        List<InventoryCompanyModel> ListCompany(int skip = 0, int limit = 100);
        InventoryCompanyModel AddCompany(InventoryCompanyModel model);
        InventoryCompanyModel UpdateCompany(InventoryCompanyModel model);
        InventoryCompanyModel GetCompany(int id);
        void DeleteInventoryCompany(int id);
    }
}
