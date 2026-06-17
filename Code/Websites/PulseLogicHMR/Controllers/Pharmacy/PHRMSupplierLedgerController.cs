using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Security;
using PulseLogicHMR.Services.Pharmacy.SupplierLedger;
using PulseLogicHMR.Utilities;
using PulseLogicHMR.ViewModel.Pharmacy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Controllers.Pharmacy
{
    [RequestFormSizeLimit(valueCountLimit: 1000000, Order = 1)]
    [PulseLogicDataFilter()]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class PHRMSupplierLedgerController : Controller
    {
        #region DECLARATIONS
        private ISupplierLedgerService _supplierLedgerService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        #endregion

        #region CTOR
        public PHRMSupplierLedgerController(ISupplierLedgerService PHRMSupplierLedgerController)
        {
            _supplierLedgerService = PHRMSupplierLedgerController;
        }
        #endregion

        #region METHODS, APIs
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                responseData.Results = await _supplierLedgerService.GetAllAsync();
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int supplierId)
        {
            try
            {
                responseData.Results = await _supplierLedgerService.GetSupplierLedgerGRDetails(supplierId);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }


        [HttpPut]
        public IActionResult MakeSupplierLedgerPayment([FromBody] IList<MakeSupplierLedgerPaymentVM> ledgerTxn)
        {
            try
            {
                var currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                responseData.Results = _supplierLedgerService.MakeSupplierLedgerPayment(ledgerTxn, currentUser);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message;
            }
            return Ok(responseData);
        }
        #endregion
    }
}
