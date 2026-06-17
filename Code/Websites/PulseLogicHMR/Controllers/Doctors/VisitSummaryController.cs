using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Enums;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace PulseLogicHMR.Controllers.Doctors
{
    public class VisitSummaryController : CommonController
    {
        private readonly DoctorsDbContext _doctorsDbContext;
        public VisitSummaryController(IOptions<MyConfiguration> _config) : base(_config)
        {
            _doctorsDbContext = new DoctorsDbContext(connString);
        }

        [HttpGet]
        [Route("VisitDetails")]
        public IActionResult VisitDetails(int visitId)
        {
            Func<object> func = () => (from data in _doctorsDbContext.VisitSummary
                                       where data.VisitId == visitId
                                       select data).ToList();
            return InvokeHttpGetFunction(func);

        }

        [HttpPost]
        [Route("VisitDetails")]
        public IActionResult PostVisitDetils()
        {
            string ipStr = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            Func<object> func = () => PostVisitDetils(ipStr, currentUser);
            return InvokeHttpPostFunction(func);

        }

        [HttpPut]
        [Route("VisitDetails")]
        public IActionResult UpdateVisitDetails()
        {
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            string ipStr = this.ReadPostData();
            Func<object> func = () => PutVisitDetails(ipStr, currentUser);
            return InvokeHttpPutFunction(func);

        }

        private object PutVisitDetails(string ipStr, RbacUser currentUser)
        {
                List<VisitSummaryModel> patDataList = PulseLogicJSONConvert.DeserializeObject<List<VisitSummaryModel>>(ipStr);

                patDataList.ForEach(data =>
                {
                    data.ModifiedOn = DateTime.Now;
                    data.ModifiedBy = currentUser.CreatedBy;
                    _doctorsDbContext.Entry(data).State = EntityState.Modified;
                    _doctorsDbContext.Entry(data).Property(u => u.CreatedBy).IsModified = false;
                    _doctorsDbContext.Entry(data).Property(u => u.CreatedOn).IsModified = false;
                });

                _doctorsDbContext.SaveChanges();
                return patDataList;

            
        }
        private object PostVisitDetils(string ipStr,RbacUser currentUser)
        {
                List<VisitSummaryModel> patDataList = PulseLogicJSONConvert.
                      DeserializeObject<List<VisitSummaryModel>>(ipStr);
            if (patDataList != null)
            {
                patDataList.ForEach(patData =>
                {
                    patData.CreatedOn = DateTime.Now;
                    patData.CreatedBy = currentUser.CreatedBy;
                    _doctorsDbContext.VisitSummary.Add(patData);
                });

                _doctorsDbContext.SaveChanges();
                return patDataList;
            }
            else
            {
                throw new Exception("Invalid Patient object.");
            }
            
        }

        // GET: api/Patient
        //[HttpGet]
        //public string Get(string reqType, int visitId)
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        //    try
        //    {
        //        DoctorsDbContext dbContext = new DoctorsDbContext(connString);
        //        {
        //            var patientDataList = (from data in dbContext.VisitSummary
        //                                   where data.VisitId == visitId
        //                                   select data).ToList();
        //            responseData.Results = patientDataList;
        //        }

        //        responseData.Status = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.Status = "Failed";
        //        responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
        //    }
        //    return PulseLogicJSONConvert.SerializeObject(responseData, true);
        //}


        // POST api/values
        //[HttpPost]
        //public string Post()
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        //    RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
        //    try
        //    {
        //        string reqType = this.ReadQueryStringData("reqType");
        //        string ipStr = this.ReadPostData();

        //        DoctorsDbContext dbContext = new DoctorsDbContext(connString);
        //        {
        //            List<VisitSummaryModel> patDataList = PulseLogicJSONConvert.
        //                  DeserializeObject<List<VisitSummaryModel>>(ipStr);
        //            if (patDataList != null)
        //            {
        //                patDataList.ForEach(patData =>
        //                {
        //                    patData.CreatedOn = DateTime.Now;
        //                    patData.CreatedBy = currentUser.CreatedBy;
        //                    dbContext.VisitSummary.Add(patData);
        //                });

        //                dbContext.SaveChanges();
        //                responseData.Results = patDataList;
        //            }
        //            else
        //                throw new Exception("Invalid Patient object.");
        //        }

        //        responseData.Status = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.Status = "Failed";
        //        responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
        //    }
        //    return PulseLogicJSONConvert.SerializeObject(responseData, true);
        //}

        // PUT api/values/5
        //[HttpPut]
        //public string Put()
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        //    RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
        //    try
        //    {
        //        string reqType = this.ReadQueryStringData("reqType");
        //        string ipStr = this.ReadPostData();

        //        DoctorsDbContext dbContext = new DoctorsDbContext(connString);
        //        {
        //            List<VisitSummaryModel> patDataList = PulseLogicJSONConvert.DeserializeObject<List<VisitSummaryModel>>(ipStr);

        //            patDataList.ForEach(data =>
        //            {
        //                data.ModifiedOn = DateTime.Now;
        //                data.ModifiedBy = currentUser.CreatedBy;
        //                dbContext.Entry(data).State = EntityState.Modified;
        //                dbContext.Entry(data).Property(u => u.CreatedBy).IsModified = false;
        //                dbContext.Entry(data).Property(u => u.CreatedOn).IsModified = false;
        //            });

        //            dbContext.SaveChanges();
        //            responseData.Results = patDataList;

        //        }
        //        responseData.Status = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.Status = "Failed";
        //        responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
        //    }
        //    return PulseLogicJSONConvert.SerializeObject(responseData, true);
        //}


        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

