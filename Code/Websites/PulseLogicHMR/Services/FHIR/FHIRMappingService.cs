using System;
using System.Collections.Generic;

namespace PulseLogicHMR.Services.FHIR
{
    public class FHIRMappingService : IFHIRMappingService
    {
        public object MapPatientToFHIR(dynamic patientModel)
        {
            if (patientModel == null) return null;

            return new
            {
                resourceType = "Patient",
                id = patientModel.PatientId.ToString(),
                identifier = new[]
                {
                    new {
                        system = "http://pulselogic.com/patient/code",
                        value = patientModel.PatientCode?.ToString()
                    },
                    new {
                        system = "https://healthid.ndhm.gov.in",
                        value = patientModel.ABHANumber?.ToString()
                    }
                },
                name = new[]
                {
                    new {
                        use = "official",
                        family = patientModel.LastName?.ToString(),
                        given = new[] { patientModel.FirstName?.ToString() }
                    }
                },
                gender = MapGender(patientModel.Gender?.ToString()),
                birthDate = patientModel.DateOfBirth != null ? Convert.ToDateTime(patientModel.DateOfBirth).ToString("yyyy-MM-dd") : null,
                telecom = new[]
                {
                    new {
                        system = "phone",
                        value = patientModel.PhoneNumber?.ToString(),
                        use = "mobile"
                    }
                }
            };
        }

        public object MapEncounterToFHIR(dynamic visitModel)
        {
            if (visitModel == null) return null;

            return new
            {
                resourceType = "Encounter",
                id = visitModel.PatientVisitId.ToString(),
                status = MapEncounterStatus(visitModel.VisitStatus?.ToString()),
                subject = new
                {
                    reference = $"Patient/{visitModel.PatientId}"
                },
                participant = new[]
                {
                    new {
                        individual = new {
                            reference = $"Practitioner/{visitModel.PerformerId}"
                        }
                    }
                },
                period = new
                {
                    start = visitModel.VisitDate != null ? Convert.ToDateTime(visitModel.VisitDate).ToString("o") : null
                }
            };
        }

        public object MapConditionToFHIR(dynamic activeMedicalModel)
        {
            if (activeMedicalModel == null) return null;

            return new
            {
                resourceType = "Condition",
                id = activeMedicalModel.PatientProblemId.ToString(),
                clinicalStatus = new {
                    coding = new[] {
                        new {
                            system = "http://terminology.hl7.org/CodeSystem/condition-clinical",
                            code = "active"
                        }
                    }
                },
                code = new {
                    coding = new[] {
                        new {
                            system = "http://hl7.org/fhir/sid/icd-10",
                            code = activeMedicalModel.ICD10Code?.ToString(),
                            display = activeMedicalModel.ICD10Description?.ToString()
                        }
                    }
                },
                subject = new {
                    reference = $"Patient/{activeMedicalModel.PatientId}"
                },
                onsetDateTime = activeMedicalModel.OnSetDate != null ? Convert.ToDateTime(activeMedicalModel.OnSetDate).ToString("yyyy-MM-dd") : null
            };
        }

        private string MapGender(string gender)
        {
            if (string.IsNullOrEmpty(gender)) return "unknown";
            string g = gender.ToLower();
            if (g == "male" || g == "m") return "male";
            if (g == "female" || g == "f") return "female";
            if (g == "other" || g == "o") return "other";
            return "unknown";
        }

        private string MapEncounterStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return "unknown";
            string s = status.ToLower();
            if (s.Contains("initiated") || s.Contains("active")) return "in-progress";
            if (s.Contains("completed") || s.Contains("concluded")) return "finished";
            if (s.Contains("cancelled")) return "cancelled";
            return "unknown";
        }
    }
}
