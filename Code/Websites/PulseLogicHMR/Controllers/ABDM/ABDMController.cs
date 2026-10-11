using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
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

                if (_config.ABDMConfig == null || string.IsNullOrEmpty(_config.ABDMConfig.ClientId) || string.IsNullOrEmpty(_config.ABDMConfig.ClientSecret))
                {
                    return StatusCode(500, new { Status = "Error", ErrorMessage = "ABDM Configuration is missing or invalid." });
                }

                string gatewayUrl = _config.ABDMConfig.GatewayUrl ?? "https://dev.abdm.gov.in/gateway/v0.5";
                string healthIdUrl = _config.ABDMConfig.HealthIdUrl ?? "https://healthidsbx.abdm.gov.in/api/v1";

                using (var client = new HttpClient())
                {
                    // 1. Authenticate to get Access Token
                    var authPayload = new
                    {
                        clientId = _config.ABDMConfig.ClientId,
                        clientSecret = _config.ABDMConfig.ClientSecret
                    };

                    var authContent = new StringContent(JsonConvert.SerializeObject(authPayload), Encoding.UTF8, "application/json");
                    var authResponse = await client.PostAsync($"{gatewayUrl}/sessions", authContent);

                    if (!authResponse.IsSuccessStatusCode)
                    {
                        string authError = await authResponse.Content.ReadAsStringAsync();
                        return StatusCode((int)authResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to authenticate with ABDM Gateway.", Details = authError });
                    }

                    var authResultString = await authResponse.Content.ReadAsStringAsync();
                    dynamic authResult = JsonConvert.DeserializeObject(authResultString);
                    string accessToken = authResult.accessToken;

                    // 2. Verify ABHA
                    var verifyPayload = new
                    {
                        healthId = abhaNumber
                    };

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    // Add standard ABDM headers (X-CM-ID might be required depending on exact endpoint)

                    var verifyContent = new StringContent(JsonConvert.SerializeObject(verifyPayload), Encoding.UTF8, "application/json");
                    // Using the standard healthid verification endpoint as per common ABDM Sandbox docs
                    var verifyResponse = await client.PostAsync($"{healthIdUrl}/search/searchByHealthId", verifyContent);

                    if (!verifyResponse.IsSuccessStatusCode)
                    {
                        string verifyError = await verifyResponse.Content.ReadAsStringAsync();
                        return StatusCode((int)verifyResponse.StatusCode, new { Status = "Error", ErrorMessage = "Failed to verify ABHA ID.", Details = verifyError });
                    }

                    var verifyResultString = await verifyResponse.Content.ReadAsStringAsync();
                    var verifyResult = JsonConvert.DeserializeObject(verifyResultString);

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
