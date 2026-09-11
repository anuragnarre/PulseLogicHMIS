using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PulseLogicHMR.Core.Configuration;

namespace PulseLogicHMR.Controllers.ABDM
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ABDMController : Controller
    {
        private readonly MyConfiguration _config;
        private readonly ABDMConfiguration _abdmConfig;

        public ABDMController(IOptions<MyConfiguration> config)
        {
            _config = config?.Value;
            _abdmConfig = _config?.ABDMConfig;
        }

        [HttpGet("VerifyABHA")]
        public async Task<IActionResult> VerifyABHA(string abhaNumber)
        {
            try
            {
                if(string.IsNullOrEmpty(abhaNumber) || abhaNumber.Length != 14)
                {
                    return BadRequest(new { Status = "Error", ErrorMessage = "Invalid ABHA Number. Must be 14 digits." });
                }

                // Integrate with the National Health Authority (NHA) ABDM Sandbox API.
                if (_abdmConfig != null && !string.IsNullOrEmpty(_abdmConfig.ClientId) && !string.IsNullOrEmpty(_abdmConfig.ClientSecret) && !string.IsNullOrEmpty(_abdmConfig.BaseUrl))
                {
                    using (var client = new HttpClient())
                    {
                        // 1. Authenticate with NHA ABDM Sandbox API
                        var authPayload = new
                        {
                            clientId = _abdmConfig.ClientId,
                            clientSecret = _abdmConfig.ClientSecret
                        };
                        var authContent = new StringContent(JsonConvert.SerializeObject(authPayload), Encoding.UTF8, "application/json");
                        var authResponse = await client.PostAsync($"{_abdmConfig.BaseUrl}/v0.5/sessions", authContent);

                        if (!authResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)authResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to authenticate with ABDM Sandbox API." });
                        }

                        var authResultString = await authResponse.Content.ReadAsStringAsync();
                        var authResult = JsonConvert.DeserializeObject<dynamic>(authResultString);
                        string accessToken = authResult?.accessToken;

                        if (string.IsNullOrEmpty(accessToken))
                        {
                            return StatusCode(500, new { Status = "Error", ErrorMessage = "Invalid authentication response from ABDM Sandbox API." });
                        }

                        // 2. Verify ABHA ID
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        var verifyPayload = new
                        {
                            healthId = abhaNumber
                        };
                        var verifyContent = new StringContent(JsonConvert.SerializeObject(verifyPayload), Encoding.UTF8, "application/json");
                        var verifyResponse = await client.PostAsync($"{_abdmConfig.BaseUrl}/v1/search/searchByHealthId", verifyContent);

                        if (verifyResponse.IsSuccessStatusCode)
                        {
                            var verifyResultString = await verifyResponse.Content.ReadAsStringAsync();
                            var verifyResult = JsonConvert.DeserializeObject<dynamic>(verifyResultString);
                            return Ok(new { Status = "OK", Results = verifyResult });
                        }
                        else
                        {
                            var errorContent = await verifyResponse.Content.ReadAsStringAsync();
                            return StatusCode((int)verifyResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to verify ABHA ID.", Details = errorContent });
                        }
                    }
                }

                // Mock Response for Proof of Concept (Phase 1 fallback)
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
