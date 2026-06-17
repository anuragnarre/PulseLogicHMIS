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
    public class CSSDSterilizationController : Controller
    {
        #region Fields
        private ICssdItemService _cssdItemService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        #endregion

        #region CTOR
        public CSSDSterilizationController(ICssdItemService cssdItemService)
        {
            _cssdItemService = cssdItemService;
        }
        #endregion

        #region Methods
        [HttpGet("GetAllPendingCSSDTransactions")]
        public async Task<IActionResult> GetAllPendingCSSDTransactions(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                responseData.Results = await _cssdItemService.GetAllPendingCSSDTransactions(FromDate, ToDate);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }
        [HttpGet("GetAllFinalizedCSSDTransactions")]
        public async Task<IActionResult> GetAllFinalizedCSSDTransactions(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                responseData.Results = await _cssdItemService.GetAllFinalizedCSSDTransactions(FromDate, ToDate);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }
        [HttpPut("DisinfectCSSDItem")]
        public async Task<IActionResult> DisinfectCSSDItem(int CssdTxnId, string DisinfectantName, string DisinfectionRemarks)
        {
            try
            {
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                responseData.Results = await _cssdItemService.DisinfectCSSDItem(CssdTxnId, DisinfectantName, DisinfectionRemarks, currentUser);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }

        [HttpPut("DispatchCSSDItem")]
        public async Task<IActionResult> DispatchCSSDItem(int CssdTxnId, string DispatchRemarks)
        {
            try
            {
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                responseData.Results = await _cssdItemService.DispatchCSSDItem(CssdTxnId, DispatchRemarks, currentUser);
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
