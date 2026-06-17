using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ViewModel.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services
{
    public interface IDesignationService
    {
        List<DesignationModel> ListDesignation();
        DesignationModel AddDesignation(DesignationModel model);
        DesignationModel UpdateDesignation(DesignationModel model);
        DesignationModel GetDesignation(int id);
    }
}
