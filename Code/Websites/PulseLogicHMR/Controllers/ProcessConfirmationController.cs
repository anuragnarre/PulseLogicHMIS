using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.Services.ProcessConfirmation;
using PulseLogicHMR.Services.ProcessConfirmation.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PulseLogicHMR.Controllers
{

    public class ProcessConfirmationController : CommonController
    {
        private readonly IProcessConfirmationService _processConfirmationService;

        public ProcessConfirmationController(IOptions<MyConfiguration> _config, IProcessConfirmationService processConfirmationService) : base(_config)
        {
            _processConfirmationService = processConfirmationService;
        }

        [HttpPost]
        [Route("ConfirmProcess")]
        public IActionResult PostConfirmProcess([FromBody] ProcessConfirmationUserCredentials_DTO processConfirmationUserCredentials)
        {
            Func<object> func = () => _processConfirmationService.ConfirmProcess(processConfirmationUserCredentials);
            return InvokeHttpPostFunction(func);
        }
    }
}
