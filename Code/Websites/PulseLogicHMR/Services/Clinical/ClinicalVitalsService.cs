using System.Collections.Generic;
using System.Linq;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.ViewModel.Clinical;
using Microsoft.Extensions.Options;

namespace PulseLogicHMR.Services.Clinical
{
    public class ClinicalVitalsService : IClinicalVitalsService
    {
        private readonly ClinicalDbContext ClinicalDbContext;

        public ClinicalVitalsService(IOptions<MyConfiguration> Config)
        {
            ClinicalDbContext = new ClinicalDbContext(Config.Value.Connectionstring);
        }

        public List<VitalsViewModel> GetVitals(int PatientVisitId)
        {
            List<VitalsViewModel> VitalsList = ClinicalDbContext.Vitals.Where(p => p.PatientVisitId == PatientVisitId).Select(v => new VitalsViewModel
            {
                VitalsTakenOn = v.VitalsTakenOn,
                PatientVisitId = v.Visit.PatientVisitId,
                PatientVitalId = v.PatientVitalId,
                PainScale = v.PainScale,
                Height = v.Height,
                HeightUnit = v.HeightUnit,
                Weight = v.Weight,
                WeightUnit = v.WeightUnit,
                Temperature = v.Temperature,
                TemperatureUnit = v.TemperatureUnit,
                BMI = v.BMI,
                Pulse = v.Pulse,
                BPDiastolic = v.BPDiastolic,
                BPSystolic = v.BPSystolic,
                RespiratoryRatePerMin = v.RespiratoryRatePerMin,
                SpO2 = v.SpO2,
                OxygenDeliveryMethod = v.OxygenDeliveryMethod,
                BodyPart = v.BodyPart,
                Advice = v.Advice,
                FreeNotes = v.FreeNotes,
                DiagnosisType = v.DiagnosisType,
                Diagnosis = v.Diagnosis,
                CreatedOn = v.CreatedOn
            }).ToList();

            if (VitalsList.Count < 3)
            {
                var PatId = ClinicalDbContext.Visit.Find(PatientVisitId).PatientId;
                var OlderPatientVisitId = ClinicalDbContext.Visit.Where(a => a.PatientId == PatId && a.PatientVisitId != PatientVisitId).Select(a => a.PatientVisitId).ToList().LastOrDefault();
                if (OlderPatientVisitId != 0)
                {
                    VitalsList.AddRange(ClinicalDbContext.Vitals.Where(p => p.PatientVisitId == OlderPatientVisitId)
                        .Select(v => new VitalsViewModel
                        {
                            VitalsTakenOn = v.VitalsTakenOn,
                            PatientVisitId = v.Visit.PatientVisitId,
                            PatientVitalId = v.PatientVitalId,
                            PainScale = v.PainScale,
                            Height = v.Height,
                            HeightUnit = v.HeightUnit,
                            Weight = v.Weight,
                            WeightUnit = v.WeightUnit,
                            Temperature = v.Temperature,
                            TemperatureUnit = v.TemperatureUnit,
                            BMI = v.BMI,
                            Pulse = v.Pulse,
                            BPDiastolic = v.BPDiastolic,
                            BPSystolic = v.BPSystolic,
                            RespiratoryRatePerMin = v.RespiratoryRatePerMin,
                            SpO2 = v.SpO2,
                            OxygenDeliveryMethod = v.OxygenDeliveryMethod,
                            BodyPart = v.BodyPart,
                            Advice = v.Advice,
                            FreeNotes = v.FreeNotes,
                            DiagnosisType = v.DiagnosisType,
                            Diagnosis = v.Diagnosis,
                            CreatedOn = v.CreatedOn
                        }).ToList());
                }
            }

            return VitalsList;
        }
    }
}
