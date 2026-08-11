/*
 File: PulseLogicCache.cs
 created: 28Jan'17-developer
 description: this class contains caching methods to be used from other classes.
 remarks: we can add more methods as per necessity
 -------------------------------------------------------------------
 change history:
 -------------------------------------------------------------------
 S.No     UpdatedBy/Date             description           remarks
 -------------------------------------------------------------------
 1.       developer/28Jan'17          created          -- sliding expiration is not considered yet.
 2.       developer/1Mar'17           modified        -- removed static constructor and hard-coded connstring
                                                      -- added this class to services.AddSingleton in startup.cs
 -------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using PulseLogicHMR.ServerModel;
using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;

namespace PulseLogicHMR.Core.Caching
{

    public enum MasterDataEnum
    {
        Department = 1,
        ICD10 = 2,
        ServiceDepartment = 3,
        Employee = 4,
        Medicine = 5,
        Reaction = 6,
        ImagingItems = 7,
        Taxes = 8,
        PastUniqueData = 9,
        LabRunNumberSettings = 10,
        PriceCategory = 11,
        AccountingCodes = 12
    }


    //this class uses MemoryCache internally.
    //write overloads to the existing methods if more parameters are needed
    public class PulseLogicCache
    {
        private static string _connString;
        private static MemoryCache _globalMemcache;
        private static int _cacheExpiryMinutes;

        public PulseLogicCache(string connectionString, int cacheExpMinutes)
        {
            _connString = connectionString;
            _globalMemcache = MemoryCache.Default;
            _cacheExpiryMinutes = cacheExpMinutes;
        }

        public static bool Add(string key, object value, DateTimeOffset absoluteExpiration)
        {
            return _globalMemcache.Add(key, value,
                 new CacheItemPolicy()
                 {
                     AbsoluteExpiration = absoluteExpiration
                 });
        }

        public static bool Add(string key, object value, int absoluteExpiryMinsFromNow)
        {
            return _globalMemcache.Add(key, value,
                 new CacheItemPolicy()
                 {
                     AbsoluteExpiration = System.DateTime.Now.AddMinutes(absoluteExpiryMinsFromNow)
                 });
        }

        public static object Get(string key)
        {
            return _globalMemcache.Get(key);
        }

        public static object GetMasterData(MasterDataEnum masterName)
        {
            CoreDbContext coreDbContext = new CoreDbContext(_connString);
            
            switch (masterName)
            {
                case MasterDataEnum.LabRunNumberSettings:
                    return GetLabRunNumberSettings(coreDbContext);
                case MasterDataEnum.PastUniqueData:
                    return GetPastUniqueData(coreDbContext);
                case MasterDataEnum.Department:
                    return GetDepartmentData(coreDbContext);
                case MasterDataEnum.ICD10:
                    return GetICD10Data(coreDbContext);
                case MasterDataEnum.Employee:
                    return GetEmployeeData(coreDbContext);
                case MasterDataEnum.Reaction:
                    return GetReactionData(coreDbContext);
                case MasterDataEnum.ImagingItems:
                    return GetImagingItemsData(coreDbContext);
                case MasterDataEnum.ServiceDepartment:
                    return GetServiceDepartmentData(coreDbContext);
                case MasterDataEnum.PriceCategory:
                    return GetPriceCategoryData(coreDbContext);
                case MasterDataEnum.Taxes:
                    return GetTaxesData(coreDbContext);
                case MasterDataEnum.Medicine:
                    return GetMedicineData(coreDbContext);
                case MasterDataEnum.AccountingCodes:
                    return GetAccountingCodesData(coreDbContext);
                default:
                    return null;
            }
        }

        private static object GetLabRunNumberSettings(CoreDbContext coreDbContext)
        {
            object returnValue = Get("lab-runnumber-settings");
            if (returnValue == null)
            {
                returnValue = coreDbContext.LabRunNumberSettings.ToList<LabRunNumberSettingsModel>();
                Add("lab-runnumber-settings", returnValue, DateTime.Now.AddMinutes(20));
            }
            return returnValue;
        }

        private static object GetPastUniqueData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("past-unique-data");
            if (returnValue == null)
            {
                UniquePastDataModel allUniqueData = new UniquePastDataModel();
                allUniqueData.UniqueAddressList = coreDbContext.Patients.Where(pat => pat.Address != null).Select(p => p.Address).Distinct().OrderBy(a => a).ToList();

                Add("past-unique-data", allUniqueData, DateTime.Now.AddHours(24));
                returnValue = allUniqueData;
            }
            return returnValue;
        }

        private static object GetDepartmentData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-departments");
            if (returnValue == null)
            {
                returnValue = coreDbContext.Departments.OrderBy(a => a.DepartmentName).ToList<DepartmentModel>();
                Add("master-departments", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetICD10Data(CoreDbContext coreDbContext)
        {
            // OPT-02 FIX: The original code queried the DB before checking the cache, making
            // the null-check unreachable. Cache was never written — all calls hit the DB.
            // Fixed to follow the same check-then-populate pattern as every other Get*Data method.
            object returnValue = Get("master-icd10");
            if (returnValue == null)
            {
                returnValue = coreDbContext.ICD10Codes.Where(icd => icd.Active == true).ToList<ICD10CodeModel>();
                Add("master-icd10", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetEmployeeData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-employee");
            if (returnValue == null)
            {
                returnValue = coreDbContext.Employees.ToList<EmployeeModel>();
                Add("master-employee", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetReactionData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-reaction");
            if (returnValue == null)
            {
                returnValue = coreDbContext.Reactions.ToList<ReactionModel>();
                Add("master-reaction", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetImagingItemsData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-imagingitem");
            if (returnValue == null)
            {
                returnValue = coreDbContext.ImagingItems.ToList<RadiologyImagingItemModel>();
                Add("master-imagingitem", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetServiceDepartmentData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-servicedepartment");
            if (returnValue == null)
            {
                returnValue = coreDbContext.ServiceDepartments.ToList<ServiceDepartmentModel>();
                Add("master-servicedepartment", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetPriceCategoryData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-pricecategory");
            if (returnValue == null)
            {
                returnValue = coreDbContext.PriceCategory.ToList<PriceCategoryModel>();
                Add("master-pricecategory", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetTaxesData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-taxes");
            if (returnValue == null)
            {
                returnValue = coreDbContext.Taxes.ToList<TaxModel>();
                Add("master-taxes", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetMedicineData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-medicines");
            if (returnValue == null)
            {
                returnValue = coreDbContext.Medicines.ToList<PHRMItemMasterModel>();
                Add("master-medicines", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }

        private static object GetAccountingCodesData(CoreDbContext coreDbContext)
        {
            object returnValue = Get("master-accounting-codes");
            if (returnValue == null)
            {
                returnValue = (from h in coreDbContext.Hospitals
                               join cod in coreDbContext.ACCCodeDetails
                               on h.HospitalId equals cod.HospitalId
                               where h.IsActive == true
                               select cod).ToList<AccountingCodeDetailsModel>();

                Add("master-accounting-codes", returnValue, DateTime.Now.AddMinutes(_cacheExpiryMinutes));
            }
            return returnValue;
        }
    }

}
