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
    public class DesignationController : Controller
    {

        public IDesignationService _designationService;
        public PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

        public DesignationController( IDesignationService designationService) 
        {
            _designationService = designationService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            try
            {
                responseData.Results = _designationService.ListDesignation(pageNumber, pageSize);
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
                responseData.Results = _designationService.GetDesignation(id);
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
        public IActionResult Post([FromBody]DesignationModel value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                value.CreatedBy = currentUser.EmployeeId;
                _designationService.AddDesignation(value);
                responseData.Results = _designationService.GetDesignation(value.DesignationId);
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
        public IActionResult Put(int id, [FromBody]DesignationModel value)
        {            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                value.DesignationId = id;
                _designationService.UpdateDesignation(value);
                responseData.Results = _designationService.GetDesignation(id);
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
