using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.FractionModels;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ViewModel.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services
{
    public interface IFractionPercentService
    {
        List<FractionPercentVM> ListFractionApplicableItems(int offset, int limit);
        FractionPercentVM AddFractionPercent(FractionPercentModel model);
        FractionPercentVM UpdateFractionPercent(FractionPercentModel model);
        FractionPercentVM GetFractionPercent(int id);
        FractionPercentVM GetFractionPercentByBillPriceId(int id);

    }
}
