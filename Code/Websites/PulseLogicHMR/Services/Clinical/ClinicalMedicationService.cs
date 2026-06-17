using System.Collections.Generic;
using System.Linq;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.ClinicalModels;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.ServerModel.PharmacyModels;

namespace PulseLogicHMR.Services.Clinical
{
    public class ClinicalMedicationService : IClinicalMedicationService
    {
        private readonly ClinicalDbContext _clinicalDbContext;
        private readonly PharmacyDbContext _pharmDbContext;
        private readonly MasterDbContext _masterDbContext;

        public ClinicalMedicationService(ClinicalDbContext clinicalDbContext, PharmacyDbContext pharmDbContext, MasterDbContext masterDbContext)
        {
            _clinicalDbContext = clinicalDbContext;
            _pharmDbContext = pharmDbContext;
            _masterDbContext = masterDbContext;
        }

        public List<HomeMedicationModel> GetHomeMedication(int patientId)
        {
            List<HomeMedicationModel> homeMedicationList = _clinicalDbContext.HomeMedications
                                        .Where(p => p.PatientId == patientId).ToList();
            
            //assigning the MedicaitonName
            List<PHRMItemMasterModel> medicationList = _pharmDbContext.PHRMItemMaster.ToList();

            foreach (var homeMed in homeMedicationList)
            {
                if (homeMed.MedicationId != 0)
                {
                    homeMed.MedicationName = medicationList
                                              .Where(a => a.ItemId == homeMed.MedicationId).FirstOrDefault()?.ItemName;
                }
            }
            return homeMedicationList;
        }

        public object GetMedicationPrescription(int patientId)
        {
            List<MedicationPrescriptionModel> prescriptionList = _clinicalDbContext.MedicationPrescriptions
                                                           .Where(p => p.PatientId == patientId).ToList();

            //assigning MedicationName and EmployeeName;
            List<PHRMItemMasterModel> medicationList = _pharmDbContext.PHRMItemMaster.ToList();
            List<EmployeeModel> employeeList = _masterDbContext.Employees.ToList();
            foreach (var pres in prescriptionList)
            {
                if (pres.MedicationId != 0)
                {
                    pres.MedicationName = medicationList
                                                .Where(a => a.ItemId == pres.MedicationId).FirstOrDefault()?.ItemName;
                }
                if (pres.PerformerId != 0)
                {
                    pres.PerformerName = employeeList
                   .Where(e => e.EmployeeId == pres.PerformerId)
                   .Select(e => e.FullName).FirstOrDefault();
                }
            }
            return prescriptionList;
        }
    }
}
