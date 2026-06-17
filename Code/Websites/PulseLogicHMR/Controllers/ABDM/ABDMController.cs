using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Core.Configuration;

namespace PulseLogicHMR.Controllers.ABDM
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ABDMController : Controller
    {
        public ABDMController()
        {
        }

        [HttpGet("VerifyABHA")]
        public IActionResult VerifyABHA(string abhaNumber)
        {
            try
            {
                // TODO: In Phase 2, integrate with the National Health Authority (NHA) ABDM Sandbox API.
                // This requires sending the ClientID and ClientSecret to authenticate, 
                // and then verifying the ABHA ID.
                
                if(string.IsNullOrEmpty(abhaNumber) || abhaNumber.Length != 14)
                {
                    return BadRequest(new { Status = "Error", ErrorMessage = "Invalid ABHA Number. Must be 14 digits." });
                }

                // Mock Response for Proof of Concept (Phase 1)
                var mockPatientData = new
                {
                    FirstName = "Ayushman",
                    LastName = "Bharat",
                    Gender = "Male",
                    YearOfBirth = "1994",
                    MonthOfBirth = "01",
                    DayOfBirth = "01",
                    Mobile = "9999999999",
                    Address = "New Delhi, India",
                    StateCode = "07",
                    DistrictCode = "094"
                };

                return Ok(new { Status = "OK", Results = mockPatientData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ErrorMessage = "Failed to verify ABHA ID. " + ex.Message });
            }
        }
    }
}
