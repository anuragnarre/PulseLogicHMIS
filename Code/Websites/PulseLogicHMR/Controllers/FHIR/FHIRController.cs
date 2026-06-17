using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.ServerModel;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Services.FHIR;

namespace PulseLogicHMR.Controllers.FHIR
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class FHIRController : CommonController
    {
        private readonly PatientDbContext _patientDbContext;
        private readonly VisitDbContext _visitDbContext;
        private readonly ClinicalDbContext _clinicalDbContext;
        private readonly IFHIRMappingService _fhirMappingService;

        public FHIRController(IOptions<MyConfiguration> _config, IFHIRMappingService fhirMappingService) : base(_config)
        {
            _patientDbContext = new PatientDbContext(connString);
            _visitDbContext = new VisitDbContext(connString);
            _clinicalDbContext = new ClinicalDbContext(connString);
            _fhirMappingService = fhirMappingService;
        }

        [HttpGet("Patient/{patientId}")]
        public IActionResult GetPatient(int patientId)
        {
            try
            {
                var patient = _patientDbContext.Patients.FirstOrDefault(p => p.PatientId == patientId);
                if (patient == null) return NotFound("Patient not found");

                var fhirPatient = _fhirMappingService.MapPatientToFHIR(patient);
                return Ok(fhirPatient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Encounter")]
        public IActionResult GetEncounters([FromQuery] int patientId)
        {
            try
            {
                var visits = _visitDbContext.Visits.Where(v => v.PatientId == patientId).ToList();
                var fhirEncounters = visits.Select(v => _fhirMappingService.MapEncounterToFHIR(v)).ToList();
                
                return Ok(new {
                    resourceType = "Bundle",
                    type = "searchset",
                    total = fhirEncounters.Count,
                    entry = fhirEncounters.Select(e => new { resource = e })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Condition")]
        public IActionResult GetConditions([FromQuery] int patientId)
        {
            try
            {
                var problems = _clinicalDbContext.ActiveMedical.Where(p => p.PatientId == patientId).ToList();
                var fhirConditions = problems.Select(p => _fhirMappingService.MapConditionToFHIR(p)).ToList();
                
                return Ok(new {
                    resourceType = "Bundle",
                    type = "searchset",
                    total = fhirConditions.Count,
                    entry = fhirConditions.Select(c => new { resource = c })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
