using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Services.ABDM;

namespace PulseLogicHMR.Controllers.ABDM
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ABDMController : Controller
    {
        private readonly IABDMService _abdmService;

        public ABDMController(IABDMService abdmService)
        {
            _abdmService = abdmService;
        }

        [HttpGet("VerifyABHA")]
        public IActionResult VerifyABHA(string abhaNumber)
        {
            try
            {
                // TODO: In Phase 2, integrate with the National Health Authority (NHA) ABDM Sandbox API.
                // This requires sending the ClientID and ClientSecret to authenticate, 
                // and then verifying the ABHA ID.
                
                var result = _abdmService.VerifyABHA(abhaNumber);

                return Ok(new { Status = "OK", Results = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Status = "Error", ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ErrorMessage = "Failed to verify ABHA ID. " + ex.Message });
            }
        }
    }
}
