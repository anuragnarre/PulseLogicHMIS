using System.Collections.Generic;
using PulseLogicHMR.ViewModel.Clinical;

namespace PulseLogicHMR.Services.Clinical
{
    public interface IClinicalVitalsService
    {
        List<VitalsViewModel> GetVitals(int PatientVisitId);
    }
}
