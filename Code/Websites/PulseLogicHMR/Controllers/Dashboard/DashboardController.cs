using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.DalLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PulseLogicHMR.Utilities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using PulseLogicHMR.CommonTypes;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
//this is the cotroller
namespace PulseLogicHMR.Controllers
{

    [RequestFormSizeLimit(valueCountLimit: 100000, Order = 1)]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class DashboardController : CommonController
    {
        // GET: api/values
        private readonly string config = null;
        public DashboardController(IOptions<MyConfiguration> _config) : base(_config)
        {
            config = _config.Value.Connectionstring;
        }




        [HttpGet]
        public string Get(int patientId, int patientVisitId)
        {
            PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
            try
            {

                responseData.Status = "OK";
                responseData.Results = "Not implemented";


            }
            catch (Exception ex)
            {
                responseData.Status = "Failed";
                responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
            }

            return PulseLogicJSONConvert.SerializeObject(responseData, true);


        }
        [HttpPost]
        public PatientModel Post()
        {

            return null;
        }

        [HttpPut]
        public PatientModel Put(int patientId)
        {

            return null;

        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

