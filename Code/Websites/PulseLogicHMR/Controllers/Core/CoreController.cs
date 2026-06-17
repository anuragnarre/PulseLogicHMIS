using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Core;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Core.Parameters;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
//this is the cotroller
namespace PulseLogicHMR.Controllers
{

    [RequestFormSizeLimit(valueCountLimit: 100000, Order = 1)]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class CoreController : CommonController
    {

        private MyConfiguration _appSettings = null;
        private readonly CoreDbContext _coreDbContext;
        private readonly AdmissionDbContext _admissionDbContext;


        public CoreController(IOptions<MyConfiguration> _config) : base(_config)
        {
            //connString = _config.Value.Connectionstring;
            _appSettings = _config.Value;
            _coreDbContext = new CoreDbContext(connString);
            _admissionDbContext = new AdmissionDbContext(base.connString);
        }

        [HttpGet]
        [Route("Lookups")]
        public IActionResult GetLookups(string inputValue)
        {
            Func<object> func = () => Lookups(inputValue);
            return InvokeHttpGetFunction(func);
        }

        [HttpGet]
        [Route("AppSettings")]
        public IActionResult GetAppSettings()
        {
            Func<object> func = () => AppSettings();
            return InvokeHttpGetFunction(func);
        }
       
        [HttpGet]
        [Route("EmployeeDatePreference")]
        public IActionResult GetEmployeeDatePreference()
        {
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
            Func<object> func = () => EmployeeDatePreference(currentUser);
            return InvokeHttpGetFunction(func);
        }

        [HttpPost]
        [Route("EmployeeDatePreference")]
        public IActionResult EmployeeDatePreference()
        {
            string ipDataStr = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
            Func<object> func = () => PostEmployeeDatePreference(ipDataStr, currentUser);
            return InvokeHttpGetFunction(func);
        }
        private object Lookups(string inputValue)
        {
            List<LookupsModel> allLookups = _coreDbContext.LookUps.ToList();
            List<LookupsModel> retList = new List<LookupsModel>();
            if (!string.IsNullOrEmpty(inputValue))
            {
                retList = allLookups.Where(a => a.ModuleName.ToLower() == inputValue.ToLower()).ToList();
            }
            else
            {
                retList = allLookups;
            }
            return retList;

        }
        private object AppSettings()
        {

            //Return only limited properties to client.
            //DO NOT return SECURE INFORMATION LIKE: CONNECTIONSTRINGS, FILELOCATIONS, etc..
            MyConfiguration retValue = new MyConfiguration();
            retValue.ApplicationVersionNum = this._appSettings.ApplicationVersionNum;
            retValue.highlightAbnormalLabResult = this._appSettings.highlightAbnormalLabResult;
            retValue.CacheExpirationMinutes = this._appSettings.CacheExpirationMinutes;
            return retValue;
        }
        
        private object EmployeeDatePreference(RbacUser currentUser)
        {

            var empPrefData = _coreDbContext.EmployeePreferences.Where(p => p.EmployeeId == currentUser.EmployeeId && p.PreferenceName == "DatePreference").FirstOrDefault();
            return empPrefData;


        }

        private object PostEmployeeDatePreference(string ipDataStr, RbacUser currentUser)
        {

            var existData = _admissionDbContext.EmployeePreferences.Where(p => p.EmployeeId == currentUser.EmployeeId && p.PreferenceName == "DatePreference").FirstOrDefault();
            EmployeePreferences empPref = new EmployeePreferences();
            if (existData == null)
            {
                empPref.CreatedOn = DateTime.Now;
                empPref.EmployeeId = currentUser.EmployeeId;
                empPref.CreatedBy = currentUser.EmployeeId;
                empPref.PreferenceValue = ipDataStr;
                empPref.PreferenceName = "DatePreference";
                empPref.IsActive = true;
                _admissionDbContext.EmployeePreferences.Add(empPref);
                _admissionDbContext.SaveChanges();
                return empPref;
            }
            else
            {
                existData.PreferenceValue = ipDataStr;
                existData.ModifiedBy = currentUser.EmployeeId;
                existData.ModifiedOn = DateTime.Now;
                _admissionDbContext.EmployeePreferences.Attach(existData);
                _admissionDbContext.Entry(existData).State = EntityState.Modified;
                _admissionDbContext.Entry(existData).Property(x => x.CreatedOn).IsModified = true;
                _admissionDbContext.Entry(existData).Property(x => x.CreatedBy).IsModified = true;
                _admissionDbContext.Entry(existData).Property(x => x.PreferenceValue).IsModified = true;
                _admissionDbContext.SaveChanges();
                return existData;
            }


        }

        //[HttpGet]
        //public string Get(string reqType, string inputValue)
        //{

        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();
        //    try
        //    {
        //        //if (reqType == "lookups")
        //        //{
        //        //    CoreDbContext dbcontext = new CoreDbContext(connString);

        //        //    List<LookupsModel> allLookups = dbcontext.LookUps.ToList();
        //        //    List<LookupsModel> retList = new List<LookupsModel>();
        //        //    if (!string.IsNullOrEmpty(inputValue))
        //        //    {
        //        //        retList = allLookups.Where(a => a.ModuleName.ToLower() == inputValue.ToLower()).ToList();
        //        //    }
        //        //    else
        //        //    {
        //        //        retList = allLookups;
        //        //    }
        //        //    responseData.Results = retList;
        //        //}
        //        //else 

        //        //if (reqType == "appSettings-limited")
        //        //{
        //        //    //Return only limited properties to client.
        //        //    //DO NOT return SECURE INFORMATION LIKE: CONNECTIONSTRINGS, FILELOCATIONS, etc..
        //        //    MyConfiguration retValue = new MyConfiguration();
        //        //    retValue.ApplicationVersionNum = this._appSettings.ApplicationVersionNum;
        //        //    retValue.highlightAbnormalLabResult = this._appSettings.highlightAbnormalLabResult;
        //        //    retValue.CacheExpirationMinutes = this._appSettings.CacheExpirationMinutes;
        //        //    responseData.Results = retValue;
        //        //}


        //        //if (reqType == "engToNepDate")
        //        //{
        //        //    NepaliDate ndate = new NepaliDate(connString);
        //        //    string retNepDate = ndate.NepaliLongDate(DateTime.Parse(value));
        //        //    responseData.Results = retNepDate;
        //        //}
        //        //{
        //        //    NepaliDate ndate = new NepaliDate(connString);
        //        //    string retEngDate = ndate.NeptoEnglishDate(value).ToString("yyyy-MM-dd");
        //        //    responseData.Results = retEngDate;
        //        //}

        //        // else
        //        {
        //            CoreDbContext dbcontext = new CoreDbContext(connString);
        //            RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
        //            var empPrefData = dbcontext.EmployeePreferences.Where(p => p.EmployeeId == currentUser.EmployeeId && p.PreferenceName == "DatePreference").FirstOrDefault();
        //            responseData.Results = empPrefData;
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




        //[HttpPost]
        //public string Post()
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();//type 'object' since we have variable return types
        //    CoreDbContext dbcontext = new CoreDbContext(connString);
        //    try
        //    {
        //        AdmissionDbContext dbContext = new AdmissionDbContext(base.connString);
        //        string str = this.ReadPostData();
        //        string reqType = this.ReadQueryStringData("reqType");
        //        RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
        //        //if (reqType == "post-emp-datepreference")
        //        //{
        //        //    var existData = dbcontext.EmployeePreferences.Where(p => p.EmployeeId == currentUser.EmployeeId && p.PreferenceName == "DatePreference").FirstOrDefault();
        //        //    EmployeePreferences empPref = new EmployeePreferences();
        //        //    if (existData == null)
        //        //    {
        //        //        empPref.CreatedOn = DateTime.Now;
        //        //        empPref.EmployeeId = currentUser.EmployeeId;
        //        //        empPref.CreatedBy = currentUser.EmployeeId;
        //        //        empPref.PreferenceValue = str;
        //        //        empPref.PreferenceName = "DatePreference";
        //        //        empPref.IsActive = true;
        //        //        dbcontext.EmployeePreferences.Add(empPref);
        //        //        dbcontext.SaveChanges();
        //        //        responseData.Status = "OK";
        //        //        responseData.Results = empPref;
        //        //    }
        //        //    else
        //        //    {
        //        //        existData.PreferenceValue = str;
        //        //        existData.ModifiedBy = currentUser.EmployeeId;
        //        //        existData.ModifiedOn = DateTime.Now;
        //        //        dbContext.EmployeePreferences.Attach(existData);
        //        //        dbContext.Entry(existData).State = EntityState.Modified;
        //        //        dbContext.Entry(existData).Property(x => x.CreatedOn).IsModified = true;
        //        //        dbContext.Entry(existData).Property(x => x.CreatedBy).IsModified = true;
        //        //        dbContext.Entry(existData).Property(x => x.PreferenceValue).IsModified = true;
        //        //        dbcontext.SaveChanges();
        //        //        responseData.Status = "OK";
        //        //        responseData.Results = existData;
        //        //    }
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.Status = "Failed";
        //        responseData.ErrorMessage = ex.Message + "exception details:" + ex.ToString();
        //    }
        //    return PulseLogicJSONConvert.SerializeObject(responseData, true);

        //}


        //[HttpPut]
        //public string Put()
        //{
        //    return null;
        //}
        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

}

