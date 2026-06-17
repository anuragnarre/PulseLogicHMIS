using System.Collections.Generic;

namespace PulseLogicHMR.Services.FHIR
{
    public interface IFHIRMappingService
    {
        object MapPatientToFHIR(dynamic patientModel);
        object MapEncounterToFHIR(dynamic visitModel);
        object MapConditionToFHIR(dynamic activeMedicalModel);
    }
}
