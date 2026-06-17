using System;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Utilities;
using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Services;
using PulseLogicHMR.Security;
using PulseLogicHMR.ViewModel;
using PulseLogicHMR.Core.Configuration;
using Microsoft.Extensions.Options;
using PulseLogicHMR.DalLayer;

namespace PulseLogicHMR.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class InventoryEmailController : CommonController
    {

        public IEmailService _emailService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

        public InventoryEmailController(IOptions<MyConfiguration> _config, IEmailService emailService) : base(_config)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hello");
        }
        
        [HttpPost]
        public IActionResult Post([FromBody]EmailViewModel value)
        {
            try
            {                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var emailList = value.EmailAddress.Split(';');
                value.EmailAddressList = new System.Collections.Generic.List<String>();
                foreach (var email in emailList)
                {
                    value.EmailAddressList.Add(email);
                }

                var apiKey = "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";

                responseData.Results = _emailService.SendEmail("info@hamshospital.org", value.EmailAddressList,"HAMS HOSPITAL", value.Subject, "", value.Content, apiKey);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }
       
    }
}
