using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ViewModel.Pharmacy;
using System;
using System.Collections.Generic;

namespace PulseLogicHMR.Services.Pharmacy.Rack
{
    public interface IRackService
    {
        List<RackViewModel> ListRack(int? skip = null, int? take = null);
        RackViewModel AddRack(PHRMRackModel model);
        RackViewModel UpdateRack(RackViewModel model);
        RackViewModel GetRack(int id);
        void DeleteRack(int id);
        List<RackViewModel> GetParentRack();
        List<RackViewModel> GetAllRack();
        List<PHRM_MAP_ItemToRack> GetAllRackItem();
        String GetDrugList(int rackId, int storeId);
        string GetStoreRackNameByItemId(int itemId);
    }
}
