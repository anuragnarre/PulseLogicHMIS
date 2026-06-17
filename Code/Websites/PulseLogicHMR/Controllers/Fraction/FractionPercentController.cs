using System;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Utilities;
using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Services;
using PulseLogicHMR.Security;

namespace PulseLogicHMR.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class FractionPercentController : Controller
    {

        public IFractionPercentService _FractionPercentService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

        public FractionPercentController(IFractionPercentService FractionPercentService)
        {
            _FractionPercentService = FractionPercentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            try
            {
                responseData.Results = _FractionPercentService.ListFractionApplicableItems();
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
        public IActionResult Get(int id)
        {
            try
            {
                responseData.Results = _FractionPercentService.GetFractionPercent(id);
                responseData.Status = "OK";

            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }

        [HttpPost]
        public IActionResult Post([FromBody]FractionPercentModel value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                value.CreatedBy = currentUser.EmployeeId;
                _FractionPercentService.AddFractionPercent(value);
                responseData.Results = _FractionPercentService.GetFractionPercent(value.PercentSettingId);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]FractionPercentModel value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                value.PercentSettingId = id;
                _FractionPercentService.UpdateFractionPercent(value);
                responseData.Results = _FractionPercentService.GetFractionPercent(id);
                responseData.Status = "OK";
            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }
            return Ok(responseData);
        }

        [HttpGet("~/api/FractionPercentByPriceId/{id}")]
        public IActionResult GetFractionPercentByBillPriceId(int id)
        {
            try
            {
                responseData.Results = _FractionPercentService.GetFractionPercentByBillPriceId(id);
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
