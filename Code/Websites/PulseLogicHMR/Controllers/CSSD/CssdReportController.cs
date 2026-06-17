using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Security;
using PulseLogicHMR.Services;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Controllers.CSSD
{
    [RequestFormSizeLimit(valueCountLimit: 1000000, Order = 1)]
    [PulseLogicDataFilter()]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class CssdReportController : Controller
    {
        #region Fields
        private ICssdReportService _cssdReportService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        #endregion

        #region CTOR
        public CssdReportController(ICssdReportService cssdReportService)
        {
            _cssdReportService = cssdReportService;
        }
        #endregion

        #region Methods, APIs
        [HttpGet("GetIntegratedCssdReport")]
        public async Task<IActionResult> GetIntegratedCssdReport(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                responseData.Results = await _cssdReportService.GetIntegratedCssdReport(FromDate, ToDate);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }
        #endregion
    }
}
