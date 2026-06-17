using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PulseLogicHMR.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ActivateInventoryController : Controller
    {
        private IActivateInventoryService _activateInventory;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

        public ActivateInventoryController(IActivateInventoryService activateInventory)
        {
            _activateInventory = activateInventory;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            try
            {
                responseData.Results = _activateInventory.GetAllInventories();
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
                responseData.Results = _activateInventory.GetInventory(id);
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
