using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.DalLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Utilities;
using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Core.Caching;
using PulseLogicHMR.Security;
using PulseLogicHMR.Controllers.Billing;

namespace PulseLogicHMR.Controllers
{
    public class DischargeSummaryController : CommonController
    {
        double cacheExpMinutes;//= 5;//this should come from configuration later on.

        public DischargeSummaryController(IOptions<MyConfiguration> _config) : base(_config)
        {
            cacheExpMinutes = _config.Value.CacheExpirationMinutes;
        }

        //[HttpGet]
        //public string Get(string reqType,
        //    int patientId, int patientVisitId,
        //    string admissionStatus, int wardId,
        //    int bedFeatureId, int ipVisitId,
        //    int bedId)
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        //    try
        //    {
        //        AdmissionDbContext dbContext = new AdmissionDbContext(base.connString);
        //        MasterDbContext masterDbContext = new MasterDbContext(base.connString);



        //        {
        //            return "1vcncncvn";
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return "vcvbcnvn";
        //    }       
        //}
    }
}
