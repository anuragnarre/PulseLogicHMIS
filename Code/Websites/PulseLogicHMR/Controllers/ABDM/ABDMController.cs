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

        public ABDMController(IOptions<MyConfiguration> config)
        {
            _config = config.Value;
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

                // Fallback to mock data if configuration is missing or UseMockData is true
                if (_config?.ABDMConfig == null || _config.ABDMConfig.UseMockData || string.IsNullOrEmpty(_config.ABDMConfig.BaseUrl))
                {
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

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_config.ABDMConfig.BaseUrl);

                    // 1. Authenticate to get session token
                    var authPayload = new
                    {
                        clientId = _config.ABDMConfig.ClientId,
                        clientSecret = _config.ABDMConfig.ClientSecret
                    };

                    var authContent = new StringContent(JsonConvert.SerializeObject(authPayload), Encoding.UTF8, "application/json");
                    var authResponse = await client.PostAsync("/v0.5/sessions", authContent);

                    if (!authResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)authResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to authenticate with ABDM." });
                    }

                    var authResultString = await authResponse.Content.ReadAsStringAsync();
                    var authResult = JsonConvert.DeserializeAnonymousType(authResultString, new { accessToken = "" });

                    // 2. Verify ABHA
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.accessToken);

                    var verifyPayload = new
                    {
                        healthId = abhaNumber
                    };

                    var verifyContent = new StringContent(JsonConvert.SerializeObject(verifyPayload), Encoding.UTF8, "application/json");
                    var verifyResponse = await client.PostAsync("/v1/search/searchByHealthId", verifyContent); // Corrected endpoint for searchByHealthId as per ABDM Sandbox v1

                    if (!verifyResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)verifyResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to verify ABHA with ABDM." });
                    }

                    var verifyResult = JsonConvert.DeserializeObject(await verifyResponse.Content.ReadAsStringAsync());

                    return Ok(new { Status = "OK", Results = verifyResult });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ErrorMessage = "Failed to verify ABHA ID. " + ex.Message });
            }
        }
    }
}
