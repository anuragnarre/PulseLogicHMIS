using System.Collections.Generic;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.DalLayer;

namespace PulseLogicHMR.Services.Clinical
{
    public interface IClinicalMedicationService
    {
        List<HomeMedicationModel> GetHomeMedication(int patientId);
        object GetMedicationPrescription(int patientId);
    }
}
