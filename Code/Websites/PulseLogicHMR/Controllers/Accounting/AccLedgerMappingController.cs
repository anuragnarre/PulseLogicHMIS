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
using RefactorThis.GraphDiff;//for entity-update.
using PulseLogicHMR.Security;
using System.Data;

using System.Collections;
using PulseLogicHMR.AccTransfer;
using PulseLogicHMR.Core.Caching;
using PulseLogicHMR.ServerModel.IncentiveModels;
using PulseLogicHMR.Core;
using PulseLogicHMR.ServerModel.AccountingModels;
using Newtonsoft.Json.Converters;
using PulseLogicHMR.Enums;
using PulseLogicHMR.ServerModel.AccountingModels.ViewModels;
using PulseLogicHMR.Controllers.Stickers.DTOs;
using PulseLogicHMR.Controllers.Accounting.DTOs;
using PulseLogicHMR.Services.Accounting.DTOs;
using PulseLogicHMR.Services.BillSettings.DTOs;
using PulseLogicHMR.Services.ClaimManagement.DTOs;
//using System.Collections;

namespace PulseLogicHMR.Controllers
{

    [RequestFormSizeLimit(valueCountLimit: 100000, Order = 1)]
    [PulseLogicDataFilter()]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class AccLedgerMappingController : CommonController
    {
        private AccountingDbContext _accountingDbContext;
        public AccLedgerMappingController(IOptions<MyConfiguration> _config) : base(_config)
        {
            //connString = _config.Value.Connectionstring;
            _accountingDbContext = new AccountingDbContext(connString);
        }

        #region HTTP GET APIs


        [HttpGet]
        [Route("BillingIncomeLedgers")]
        public IActionResult GetBillingIncomeLedgers()
        {
            int currentHospitalId = HttpContext.Session.Get<int>(ENUM_SessionValues.CurrentHospitalId);
            Func<object> func = () => GetBillingItemLedgers(currentHospitalId);
            return InvokeHttpGetFunction<object>(func);
        }
        #endregion HTTP GET APIs

        #region HTTP POST APIs

        [HttpPost]
        [Route("MapBillingIncomeLedger")]
        public IActionResult MapBillingIncomeLedger([FromBody] AccBillingLedgerMapping_DTO accBillingLedgerMapping)
        {
            
            int currentHospitalId = HttpContext.Session.Get<int>(ENUM_SessionValues.CurrentHospitalId);
            Func<object> func = () => MapBillingIncomeLedger(currentHospitalId, accBillingLedgerMapping);
            return InvokeHttpPostFunction<object>(func);
        }

        #endregion HTTP POST APIs

        #region HTTP PUT APIs
        [HttpPut]
        [Route("ActivateDeactivateBillingLedgerMapping")]
        public IActionResult ActivateDeactivateBillingLedgerMapping( int BillLedgerMappingId , bool IsActive)
        {
            Func<object> func = () =>UpdateActivateDeactivateBillingLedgerMapping(BillLedgerMappingId, IsActive);
            return InvokeHttpPutFunction(func);
        }


        [HttpPut]
        [Route("MapBillingLedger")]
        public IActionResult MapBillingLedger([FromBody] AccBillingLedgerMapping_DTO accBillingLedgerMapping)
        {
            Func<object> func = () => UpdateMapBillingIncomeLedger(accBillingLedgerMapping);
            return InvokeHttpPutFunction(func);
        }
        #endregion HTTP PUT APIs

        private object GetBillingItemLedgers(int currentHospitalId)
        {
            List<SqlParameter> paramList = new List<SqlParameter>() { };
            DataSet mappingDetail = DALFunctions.GetDatasetFromStoredProc("SP_ACC_GetIncomeLedgerMappingDetail", paramList, _accountingDbContext);
            DataTable mapTable = mappingDetail.Tables[0];
            var IncomeLedgerMapping = AccBillingLedgerMapping_DTO.MapDataTableToSingleObject(mapTable);
            return IncomeLedgerMapping;
        }

        private object MapBillingIncomeLedger(int HospitalId, AccBillingLedgerMapping_DTO accBillingLedgerMapping)
        {
            AccountingBillLedgerMappingModel accBillLedgerMap = new AccountingBillLedgerMappingModel();
            accBillLedgerMap.LedgerId = accBillingLedgerMapping.LedgerId;
            accBillLedgerMap.SubLedgerId = accBillingLedgerMapping.SubLedgerId == 0 ? null : accBillingLedgerMapping.SubLedgerId;
            accBillLedgerMap.ItemId = accBillingLedgerMapping.ItemId;
            accBillLedgerMap.HospitalId = HospitalId;
            accBillLedgerMap.ServiceDepartmentId = accBillingLedgerMapping.ServiceDepartmentId;
            accBillLedgerMap.IsActive = true;
            accBillLedgerMap.BillingType = accBillingLedgerMapping.BillingType;
            _accountingDbContext.AccountBillLedgerMapping.Add(accBillLedgerMap);
            _accountingDbContext.SaveChanges();
            return accBillLedgerMap;
        }

        private object UpdateActivateDeactivateBillingLedgerMapping(int BillLedgerMappingId, bool IsActive)
        {
            var ledger = _accountingDbContext.AccountBillLedgerMapping.Where(x => x.BillLedgerMappingId == BillLedgerMappingId).FirstOrDefault();
            if (ledger != null)
            {
                ledger.IsActive = IsActive; 
              
            }
            _accountingDbContext.SaveChanges();
            return ledger;
        }

        private object UpdateMapBillingIncomeLedger(AccBillingLedgerMapping_DTO accBillingLedgerMapping)
        {
            var ledgerToUpdate = _accountingDbContext.AccountBillLedgerMapping.Where(x => x.BillLedgerMappingId == accBillingLedgerMapping.BillLedgerMappingId).FirstOrDefault();
            ledgerToUpdate.SubLedgerId = accBillingLedgerMapping.SubLedgerId ==  0 ? null : accBillingLedgerMapping.SubLedgerId;
            ledgerToUpdate.LedgerId = accBillingLedgerMapping.LedgerId;
            ledgerToUpdate.ServiceDepartmentId = accBillingLedgerMapping.ServiceDepartmentId;
            ledgerToUpdate.ItemId = accBillingLedgerMapping.ItemId;
            _accountingDbContext.SaveChanges();
            return Ok();
        }
    }

}


