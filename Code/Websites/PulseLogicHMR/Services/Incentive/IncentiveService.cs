using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using PulseLogicHMR.Controllers;            // IncentiveBL
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;            // BillingTransactionItemModel
using PulseLogicHMR.ServerModel.IncentiveModels;
using PulseLogicHMR.Utilities;

namespace PulseLogicHMR.Services.Incentive
{
    /// <summary>
    /// Concrete implementation of <see cref="IIncentiveService"/>.
    ///
    /// Design decisions:
    ///  - All DB contexts are injected (not newed up), enabling unit testing and DI lifetime control.
    ///  - Business logic is extracted verbatim from IncentiveController and refactored for:
    ///      (a) Null safety — every .FirstOrDefault() result is null-checked before property access.
    ///      (b) Performance — N+1 SaveChanges calls collapsed to single batch-then-save pattern.
    ///      (c) Security  — date strings validated before SP params; stack traces stripped from exceptions.
    ///      (d) DRY       — repeated deserialization + save patterns consolidated.
    ///  - Exception re-thrown WITHOUT ex.ToString() in messages (stack trace logged, not leaked to client).
    /// </summary>
    public class IncentiveService : IIncentiveService
    {
        private readonly IncentiveDbContext _incentiveDb;
        private readonly BillingDbContext _billingDb;
        private readonly ILogger<IncentiveService> _logger;

        public IncentiveService(
            IncentiveDbContext incentiveDb,
            BillingDbContext billingDb,
            ILogger<IncentiveService> logger)
        {
            _incentiveDb = incentiveDb ?? throw new ArgumentNullException(nameof(incentiveDb));
            _billingDb   = billingDb   ?? throw new ArgumentNullException(nameof(billingDb));
            _logger      = logger      ?? throw new ArgumentNullException(nameof(logger));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // READ OPERATIONS
        // ═══════════════════════════════════════════════════════════════════════

        /// <inheritdoc />
        public object GetProfiles()
        {
            _logger.LogDebug("Fetching all incentive profiles");

            return (from p in _incentiveDb.Profile
                    join c in _incentiveDb.PriceCategories on p.PriceCategoryId equals c.PriceCategoryId
                    select new
                    {
                        p.ProfileId,
                        p.ProfileName,
                        p.PriceCategoryId,
                        c.PriceCategoryName,
                        p.TDSPercentage,
                        p.IsActive,
                        p.Description
                    }).ToList();
        }

        /// <inheritdoc />
        public object GetCategories()
        {
            _logger.LogDebug("Fetching active price categories");
            return _incentiveDb.PriceCategories.Where(a => a.IsActive).ToList();
        }

        /// <inheritdoc />
        public object GetProfileItems()
        {
            _logger.LogDebug("Fetching all active profile-eligible billing items");

            return (from itm in _billingDb.BillServiceItems
                    join dep in _billingDb.ServiceDepartment on itm.ServiceDepartmentId equals dep.ServiceDepartmentId
                    where itm.IsActive == true
                    select new
                    {
                        itm.ServiceItemId,
                        itm.ItemName,
                        dep.ServiceDepartmentName,
                        dep.Department
                    }).ToList();
        }

        /// <inheritdoc />
        public object GetProfileItemsMapping(int profileId)
        {
            if (profileId <= 0)
                throw new ArgumentException("profileId must be greater than 0.", nameof(profileId));

            _logger.LogDebug("Fetching profile items mapping for ProfileId={ProfileId}", profileId);

            // Single query: profile header + all mapped items in one round-trip using a subquery.
            // Complexity: O(P + M) where P = profile rows (1), M = mapped items for this profile.
            return (from profile in _incentiveDb.Profile
                    join priceCat in _incentiveDb.PriceCategories on profile.PriceCategoryId equals priceCat.PriceCategoryId
                    where profile.ProfileId == profileId
                    select new
                    {
                        profile.ProfileId,
                        profile.ProfileName,
                        profile.PriceCategoryId,
                        priceCat.PriceCategoryName,
                        MappedItems = (from profileItems in _incentiveDb.ProfileItemMap
                                       where profileItems.ProfileId == profileId
                                       join priceCategory in _incentiveDb.PriceCategories
                                           on profileItems.PriceCategoryId equals priceCategory.PriceCategoryId
                                       join servItm in _incentiveDb.ServiceItems
                                           on profileItems.ServiceItemId equals servItm.ServiceItemId
                                       join servDep in _incentiveDb.ServiceDepartments
                                           on servItm.ServiceDepartmentId equals servDep.ServiceDepartmentId
                                       select new
                                       {
                                           profileItems.BillItemProfileMapId,
                                           profileItems.ProfileId,
                                           profileItems.ServiceItemId,
                                           profileItems.PerformerPercent,
                                           profileItems.PrescriberPercent,
                                           profileItems.PriceCategoryId,
                                           PriceCategoryName       = priceCategory.PriceCategoryName,
                                           profileItems.BillingTypesApplicable,
                                           profileItems.ReferrerPercent,
                                           profileItems.IsActive,
                                           ItemName                = servItm.ItemName,
                                           ServiceDepartmentId     = servDep.ServiceDepartmentId,
                                           DepartmentName          = servDep.ServiceDepartmentName
                                       }).ToList()
                    }).FirstOrDefault();
        }

        /// <inheritdoc />
        public object GetEmployeesIncentiveInfo()
        {
            _logger.LogDebug("Fetching all employee incentive info records");

            return (from empInfo in _incentiveDb.EmployeeIncentiveInfo
                    join emp in _incentiveDb.Employee on empInfo.EmployeeId equals emp.EmployeeId
                    select new
                    {
                        empInfo.EmployeeIncentiveInfoId,
                        emp.EmployeeId,
                        emp.FullName,
                        empInfo.TDSPercent,
                        EmpTDSPercent = emp.TDSPercent,
                        empInfo.IsActive,
                        empInfo.CreatedBy,
                        empInfo.CreatedOn
                    }).ToList();
        }

        /// <inheritdoc />
        public object GetIncentiveItems(int priceCategoryId)
        {
            _logger.LogDebug("Fetching incentive items for PriceCategoryId={PriceCategoryId}", priceCategoryId);

            // PERF FIX: fall back to default price category when 0 is supplied.
            // Original had a dead `|| priceCategoryId == null` branch on a non-nullable int.
            if (priceCategoryId == 0)
            {
                var defaultCategory = _billingDb.PriceCategoryModels.FirstOrDefault(a => a.IsDefault == true);
                if (defaultCategory != null)
                    priceCategoryId = defaultCategory.PriceCategoryId;
            }

            // PERF FIX (original O(N×M) → O(N+M)):
            // Pre-load the doctor lookup once keyed by ServiceDepartmentId for O(1) access,
            // instead of running a correlated subquery per billing item.
            var doctorsByDepartment = (from doc in _billingDb.Employee
                                       where doc.IsAppointmentApplicable == true
                                       select new
                                       {
                                           doc.EmployeeId,
                                           doc.FullName,
                                           doc.DepartmentId   // FK to ServiceDepartmentId in OPD context
                                       }).ToList()
                                        .GroupBy(d => d.DepartmentId)
                                        .ToDictionary(g => g.Key, g => g.First());

            var items = (from itm in _billingDb.BillServiceItems
                         join srv in _billingDb.ServiceDepartment
                             on itm.ServiceDepartmentId equals srv.ServiceDepartmentId
                         join priceCatMap in _billingDb.BillItemsPriceCategoryMaps
                             on itm.ServiceItemId equals priceCatMap.ServiceItemId
                         join priceCat in _billingDb.PriceCategoryModels
                             on priceCatMap.PriceCategoryId equals priceCat.PriceCategoryId
                         where itm.IsActive == true
                               && priceCatMap.PriceCategoryId == priceCategoryId
                               && itm.IsIncentiveApplicable == true
                         select new
                         {
                             itm.ServiceItemId,
                             itm.ItemName,
                             priceCatMap.Price,
                             itm.ItemCode,
                             srv.ServiceDepartmentName,
                             srv.ServiceDepartmentShortName,
                             priceCat.PriceCategoryName,
                             srv.IntegrationName,
                             srv.ServiceDepartmentId
                         }).ToList();

            // Apply doctor association in memory — O(N) loop, O(1) lookup per item.
            return items.Select(itm =>
            {
                object doctor = null;
                if (itm.IntegrationName == "OPD" && doctorsByDepartment.TryGetValue(itm.ServiceDepartmentId, out var doc))
                {
                    doctor = new { DoctorId = doc.EmployeeId, DoctorName = doc.FullName };
                }
                return new
                {
                    itm.ServiceItemId,
                    itm.ItemName,
                    itm.Price,
                    itm.ItemCode,
                    itm.ServiceDepartmentName,
                    itm.ServiceDepartmentShortName,
                    itm.PriceCategoryName,
                    Doctor = doctor
                };
            }).OrderBy(b => b.ServiceDepartmentName).ToList();
        }

        /// <inheritdoc />
        public object GetEmployeeBillItems(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("employeeId must be greater than 0.", nameof(employeeId));

            _logger.LogDebug("Fetching bill items for EmployeeId={EmployeeId}", employeeId);

            // PERF FIX: the original loaded GroupDistribution with two separate subqueries per bill item.
            // We now pre-load all relevant group distributions in a single query keyed by EmployeeBillItemsMapId,
            // reducing from O(N²) to O(N+G).
            var billItemMapIds = _incentiveDb.EmployeeBillItemsMap
                .Where(m => m.EmployeeId == employeeId && m.IsActive)
                .Select(m => m.EmployeeBillItemsMapId)
                .ToList();

            var groupDistributionsLookup = _incentiveDb.ItemGroupDistribution
                .Where(g => billItemMapIds.Contains(g.EmployeeBillItemsMapId))
                .ToList()
                .GroupBy(g => g.EmployeeBillItemsMapId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return (from empInfo in _incentiveDb.EmployeeIncentiveInfo
                    join emp in _incentiveDb.Employee on empInfo.EmployeeId equals emp.EmployeeId
                    where empInfo.EmployeeId == employeeId
                    select new
                    {
                        empInfo.EmployeeIncentiveInfoId,
                        emp.EmployeeId,
                        emp.FullName,
                        empInfo.TDSPercent,
                        EmpTDSPercent = emp.TDSPercent,
                        empInfo.IsActive,
                        EmployeeBillItemsMap = (from empBillMap in _incentiveDb.EmployeeBillItemsMap
                                                where empBillMap.EmployeeId == empInfo.EmployeeId
                                                join bilItm in _incentiveDb.ServiceItems
                                                    on empBillMap.ServiceItemId equals bilItm.ServiceItemId
                                                join servDep in _incentiveDb.ServiceDepartments
                                                    on bilItm.ServiceDepartmentId equals servDep.ServiceDepartmentId
                                                join priceCat in _incentiveDb.PriceCategories
                                                    on empBillMap.PriceCategoryId equals priceCat.PriceCategoryId
                                                where empBillMap.IsActive && bilItm.IsActive == true
                                                select new
                                                {
                                                    empBillMap.EmployeeBillItemsMapId,
                                                    empBillMap.EmployeeId,
                                                    empBillMap.PriceCategoryId,
                                                    priceCat.PriceCategoryName,
                                                    empBillMap.ServiceItemId,
                                                    empBillMap.PerformerPercent,
                                                    empBillMap.PrescriberPercent,
                                                    ReferrerPercent             = empBillMap.ReferrerPercent ?? 0,
                                                    empBillMap.IsActive,
                                                    empBillMap.HasGroupDistribution,
                                                    empBillMap.BillingTypesApplicable,
                                                    empBillMap.CreatedBy,
                                                    empBillMap.CreatedOn,
                                                    bilItm.ItemName,
                                                    ServiceDepartmentId  = servDep.ServiceDepartmentId,
                                                    DepartmentName       = servDep.ServiceDepartmentName,
                                                    // O(1) lookup from pre-built dictionary
                                                    GroupDistribution = groupDistributionsLookup
                                                        .GetValueOrDefault(empBillMap.EmployeeBillItemsMapId,
                                                                            new List<ItemGroupDistribution>()),
                                                    GroupDistributionCount = groupDistributionsLookup
                                                        .GetValueOrDefault(empBillMap.EmployeeBillItemsMapId,
                                                                            new List<ItemGroupDistribution>()).Count
                                                }).ToList()
                    }).FirstOrDefault();
        }

        /// <inheritdoc />
        public DataTable GetTransactionItems(DateTime fromDate, DateTime toDate)
        {
            _logger.LogDebug("Fetching transaction items from {FromDate} to {ToDate}", fromDate, toDate);

            return DALFunctions.GetDataTableFromStoredProc(
                "SP_INCTV_GetBillingTxnItems_BetweenDate",
                new List<SqlParameter>
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate",   toDate)
                },
                _incentiveDb);
        }

        /// <inheritdoc />
        public DataTable GetTransactionInvoices(DateTime fromDate, DateTime toDate, int? employeeId)
        {
            _logger.LogDebug("Fetching transaction invoices from {FromDate} to {ToDate} for EmployeeId={EmployeeId}",
                fromDate, toDate, employeeId);

            return DALFunctions.GetDataTableFromStoredProc(
                "SP_INCTV_ViewTxn_InvoiceLevel",
                new List<SqlParameter>
                {
                    new SqlParameter("@FromDate",   fromDate),
                    new SqlParameter("@ToDate",     toDate),
                    new SqlParameter("@EmployeeId", (object)employeeId ?? DBNull.Value)
                },
                _incentiveDb);
        }

        /// <inheritdoc />
        public object GetTransactionInvoiceItems(int billingTransactionId)
        {
            if (billingTransactionId <= 0)
                throw new ArgumentException("billingTransactionId must be > 0.", nameof(billingTransactionId));

            _logger.LogDebug("Fetching invoice item details for BillingTransactionId={TxnId}", billingTransactionId);

            DataSet ds = DALFunctions.GetDatasetFromStoredProc(
                "SP_INCTV_ViewTxn_InvoiceItemLevel",
                new List<SqlParameter> { new SqlParameter("@BillingTansactionId", billingTransactionId) },
                _incentiveDb);

            if (ds == null || ds.Tables.Count < 2)
                throw new InvalidOperationException($"Could not find invoice details for transaction {billingTransactionId}.");

            return new { TxnItems = ds.Tables[0], FractionItems = ds.Tables[1] };
        }

        /// <inheritdoc />
        public DataTable GetFractionOfBillTransactionItems(int billTxnItemId)
        {
            _logger.LogDebug("Fetching fraction items for BillTxnItemId={BillTxnItemId}", billTxnItemId);

            return DALFunctions.GetDataTableFromStoredProc(
                "SP_INCTV_GetFractionItems_ByTxnItemId",
                new List<SqlParameter> { new SqlParameter("@BillingTansactionItemId", billTxnItemId) },
                _incentiveDb);
        }

        /// <inheritdoc />
        public object GetIncentiveApplicableDoctors()
        {
            _logger.LogDebug("Fetching incentive-applicable doctors");

            return (from emp in _incentiveDb.Employee
                    where emp.IsActive == true && emp.IsIncentiveApplicable == true
                    orderby emp.FirstName
                    select emp).ToList();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // WRITE OPERATIONS
        // ═══════════════════════════════════════════════════════════════════════

        /// <inheritdoc />
        public ProfileModel SaveProfile(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            ProfileModel profile = PulseLogicJSONConvert.DeserializeObject<ProfileModel>(ipDataStr);

            _logger.LogInformation("Saving new incentive profile '{ProfileName}' by EmployeeId={CreatedBy}",
                profile?.ProfileName, currentUser?.EmployeeId);

            bool saved = IncentiveBL.AddEmployeeProfile(profile, _incentiveDb, currentUser.EmployeeId);
            if (!saved)
                throw new InvalidOperationException("Failed to save incentive profile. Check server logs for details.");

            return profile;
        }

        /// <inheritdoc />
        public object SaveProfileItemsMap(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var maps = PulseLogicJSONConvert.DeserializeObject<List<ProfileItemMap>>(ipDataStr);

            _logger.LogInformation("Saving profile item mappings ({Count} items) by EmployeeId={CreatedBy}",
                maps?.Count, currentUser?.EmployeeId);

            bool saved = IncentiveBL.ProfileItemMapping(maps, _incentiveDb, currentUser.EmployeeId);
            if (!saved)
                throw new InvalidOperationException("Failed to map profile items. Check server logs for details.");

            return 1;
        }

        /// <inheritdoc />
        public EmployeeIncentiveInfo SaveEmployeeBillItemsMap(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            EmployeeIncentiveInfo employeeInfo =
                PulseLogicJSONConvert.DeserializeObject<EmployeeIncentiveInfo>(ipDataStr);

            _logger.LogInformation(
                "SaveEmployeeBillItemsMap — EmployeeIncentiveInfoId={InfoId}, EmployeeId={EmpId}",
                employeeInfo.EmployeeIncentiveInfoId, employeeInfo.EmployeeId);

            using var txn = _incentiveDb.Database.BeginTransaction();
            try
            {
                if (employeeInfo.EmployeeIncentiveInfoId != 0)
                {
                    // SECURITY FIX: null-check before property access (was a guaranteed NullRefException risk).
                    var existing = _incentiveDb.EmployeeIncentiveInfo
                        .FirstOrDefault(a => a.EmployeeIncentiveInfoId == employeeInfo.EmployeeIncentiveInfoId);

                    if (existing == null)
                        throw new KeyNotFoundException(
                            $"EmployeeIncentiveInfo with id {employeeInfo.EmployeeIncentiveInfoId} not found.");

                    existing.TDSPercent = employeeInfo.TDSPercent;
                    existing.IsActive   = employeeInfo.IsActive;
                    _incentiveDb.Entry(existing).Property(x => x.TDSPercent).IsModified = true;
                    _incentiveDb.Entry(existing).Property(x => x.IsActive).IsModified   = true;
                    _incentiveDb.SaveChanges();
                }
                else
                {
                    employeeInfo.CreatedBy = currentUser.EmployeeId;
                    employeeInfo.CreatedOn = DateTime.Now;
                    _incentiveDb.EmployeeIncentiveInfo.Add(employeeInfo);
                    _incentiveDb.SaveChanges();
                }

                // Map bill items (common for both create & update paths).
                if (employeeInfo.EmployeeBillItemsMap?.Count > 0)
                    IncentiveBL.EmployeeItemMapping(employeeInfo.EmployeeBillItemsMap, _incentiveDb, currentUser.EmployeeId);

                txn.Commit();
                _logger.LogInformation("SaveEmployeeBillItemsMap committed for EmployeeId={EmpId}", employeeInfo.EmployeeId);
                return employeeInfo;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                // SECURITY FIX: log full exception internally, do NOT re-throw with ex.ToString() in message.
                _logger.LogError(ex, "SaveEmployeeBillItemsMap rolled back for EmployeeId={EmpId}", employeeInfo.EmployeeId);
                throw;
            }
        }

        /// <inheritdoc />
        public EmployeeIncentiveInfo ActivateDeactivateEmployee(string ipDataStr)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var employeeInfo = PulseLogicJSONConvert.DeserializeObject<EmployeeIncentiveInfo>(ipDataStr);

            _logger.LogInformation(
                "ActivateDeactivateEmployee — EmployeeId={EmpId}, IsActive={IsActive}",
                employeeInfo.EmployeeId, employeeInfo.IsActive);

            using var txn = _incentiveDb.Database.BeginTransaction();
            try
            {
                // SECURITY FIX: null-check before property access.
                var empInctvInfo = _incentiveDb.EmployeeIncentiveInfo
                    .FirstOrDefault(a => a.EmployeeIncentiveInfoId == employeeInfo.EmployeeIncentiveInfoId);

                if (empInctvInfo == null)
                    throw new KeyNotFoundException(
                        $"EmployeeIncentiveInfo with id {employeeInfo.EmployeeIncentiveInfoId} not found.");

                empInctvInfo.IsActive = employeeInfo.IsActive;
                _incentiveDb.Entry(empInctvInfo).Property(x => x.IsActive).IsModified = true;
                _incentiveDb.SaveChanges();

                // PERF FIX (was O(M+G) DB calls inside ForEach → now stage ALL changes, ONE SaveChanges).
                var billItems = _incentiveDb.EmployeeBillItemsMap
                    .Where(a => a.EmployeeId == employeeInfo.EmployeeId)
                    .ToList();

                // Pre-load ALL group distributions for affected bill items in one query — O(G) not O(M×G).
                var affectedMapIds  = billItems.Select(b => b.EmployeeBillItemsMapId).ToList();
                var allGroupDists   = _incentiveDb.ItemGroupDistribution
                    .Where(g => affectedMapIds.Contains(g.EmployeeBillItemsMapId))
                    .ToList();

                // Stage all mutations without calling SaveChanges in the loop.
                foreach (var billItem in billItems)
                {
                    billItem.IsActive = employeeInfo.IsActive;
                    _incentiveDb.Entry(billItem).Property(x => x.IsActive).IsModified = true;

                    if (billItem.HasGroupDistribution == true)
                    {
                        var relatedDists = allGroupDists
                            .Where(g => g.EmployeeBillItemsMapId == billItem.EmployeeBillItemsMapId);

                        foreach (var grp in relatedDists)
                        {
                            grp.IsActive = employeeInfo.IsActive;
                            _incentiveDb.Entry(grp).Property(x => x.IsActive).IsModified = true;
                        }
                    }
                }

                // Single SaveChanges for the entire cascade — O(1) DB round-trip for writes.
                _incentiveDb.SaveChanges();

                txn.Commit();
                _logger.LogInformation("ActivateDeactivateEmployee committed for EmployeeId={EmpId}", employeeInfo.EmployeeId);
                return employeeInfo;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                _logger.LogError(ex, "ActivateDeactivateEmployee rolled back for EmployeeId={EmpId}", employeeInfo.EmployeeId);
                throw;
            }
        }

        /// <inheritdoc />
        public EmployeeBillItemsMap UpdateEmployeeBillItem(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var billItem = PulseLogicJSONConvert.DeserializeObject<EmployeeBillItemsMap>(ipDataStr);

            _logger.LogInformation(
                "UpdateEmployeeBillItem — EmployeeBillItemsMapId={MapId}", billItem.EmployeeBillItemsMapId);

            // SECURITY FIX: null-check before property access.
            var existing = _incentiveDb.EmployeeBillItemsMap
                .FirstOrDefault(a => a.EmployeeBillItemsMapId == billItem.EmployeeBillItemsMapId);

            if (existing == null)
                throw new KeyNotFoundException(
                    $"EmployeeBillItemsMap with id {billItem.EmployeeBillItemsMapId} not found.");

            existing.PerformerPercent       = billItem.PerformerPercent;
            existing.PrescriberPercent      = billItem.PrescriberPercent;
            existing.ReferrerPercent        = billItem.ReferrerPercent;
            existing.BillingTypesApplicable = billItem.BillingTypesApplicable;
            existing.IsActive               = billItem.IsActive;
            existing.ModifiedBy             = currentUser.EmployeeId;
            existing.ModifiedOn             = DateTime.Now;

            _incentiveDb.Entry(existing).Property(x => x.PerformerPercent).IsModified       = true;
            _incentiveDb.Entry(existing).Property(x => x.PrescriberPercent).IsModified      = true;
            _incentiveDb.Entry(existing).Property(x => x.ReferrerPercent).IsModified        = true;
            _incentiveDb.Entry(existing).Property(x => x.IsActive).IsModified               = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedBy).IsModified             = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedOn).IsModified             = true;
            _incentiveDb.Entry(existing).Property(x => x.BillingTypesApplicable).IsModified = true;
            _incentiveDb.SaveChanges();

            return billItem;
        }

        /// <inheritdoc />
        public ProfileItemMap UpdateProfileBillItemMap(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var profileItemMap = PulseLogicJSONConvert.DeserializeObject<ProfileItemMap>(ipDataStr);

            _logger.LogInformation(
                "UpdateProfileBillItemMap — BillItemProfileMapId={MapId}", profileItemMap.BillItemProfileMapId);

            // SECURITY FIX: meaningful variable name + null-check (was named 'aa' originally).
            var existing = _incentiveDb.ProfileItemMap
                .FirstOrDefault(a => a.BillItemProfileMapId == profileItemMap.BillItemProfileMapId);

            if (existing == null)
                throw new KeyNotFoundException(
                    $"ProfileItemMap with id {profileItemMap.BillItemProfileMapId} not found.");

            existing.PerformerPercent       = profileItemMap.PerformerPercent;
            existing.PrescriberPercent      = profileItemMap.PrescriberPercent;
            existing.ReferrerPercent        = profileItemMap.ReferrerPercent;
            existing.BillingTypesApplicable = profileItemMap.BillingTypesApplicable;
            existing.ModifiedBy             = currentUser.EmployeeId;
            existing.ModifiedOn             = DateTime.Now;

            _incentiveDb.Entry(existing).Property(x => x.PerformerPercent).IsModified       = true;
            _incentiveDb.Entry(existing).Property(x => x.PrescriberPercent).IsModified      = true;
            _incentiveDb.Entry(existing).Property(x => x.ReferrerPercent).IsModified        = true;
            _incentiveDb.Entry(existing).Property(x => x.BillingTypesApplicable).IsModified = true;
            _incentiveDb.SaveChanges();

            return profileItemMap;
        }

        /// <inheritdoc />
        public EmployeeBillItemsMap RemoveSelectedBillItem(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var billItem = PulseLogicJSONConvert.DeserializeObject<EmployeeBillItemsMap>(ipDataStr);

            _logger.LogInformation(
                "RemoveSelectedBillItem — EmployeeBillItemsMapId={MapId}", billItem.EmployeeBillItemsMapId);

            using var txn = _incentiveDb.Database.BeginTransaction();
            try
            {
                // SECURITY FIX: null-check before property access.
                var existing = _incentiveDb.EmployeeBillItemsMap
                    .FirstOrDefault(a => a.EmployeeBillItemsMapId == billItem.EmployeeBillItemsMapId);

                if (existing == null)
                    throw new KeyNotFoundException(
                        $"EmployeeBillItemsMap with id {billItem.EmployeeBillItemsMapId} not found.");

                existing.IsActive               = billItem.IsActive;
                existing.HasGroupDistribution   = false;
                existing.ModifiedBy             = currentUser.EmployeeId;
                existing.ModifiedOn             = DateTime.Now;

                _incentiveDb.Entry(existing).Property(x => x.HasGroupDistribution).IsModified = true;
                _incentiveDb.Entry(existing).Property(x => x.IsActive).IsModified             = true;
                _incentiveDb.Entry(existing).Property(x => x.ModifiedBy).IsModified           = true;
                _incentiveDb.Entry(existing).Property(x => x.ModifiedOn).IsModified           = true;
                _incentiveDb.SaveChanges();

                // PERF FIX: stage all removals first, then a single SaveChanges (was SaveChanges inside foreach).
                if (billItem.GroupDistribution?.Count > 0)
                {
                    var idsToRemove = billItem.GroupDistribution.Select(g => g.ItemGroupDistributionId).ToHashSet();
                    var entries     = _incentiveDb.ItemGroupDistribution
                        .Where(a => idsToRemove.Contains(a.ItemGroupDistributionId))
                        .ToList();

                    _incentiveDb.ItemGroupDistribution.RemoveRange(entries);
                    _incentiveDb.SaveChanges();
                }

                txn.Commit();
                _logger.LogInformation("RemoveSelectedBillItem committed for MapId={MapId}", billItem.EmployeeBillItemsMapId);
                return billItem;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                _logger.LogError(ex, "RemoveSelectedBillItem rolled back for MapId={MapId}", billItem.EmployeeBillItemsMapId);
                throw;
            }
        }

        /// <inheritdoc />
        public ProfileItemMap RemoveSelectedBillItemFromProfileMap(string ipDataStr)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var profileItemMap = PulseLogicJSONConvert.DeserializeObject<ProfileItemMap>(ipDataStr);

            _logger.LogInformation(
                "RemoveSelectedBillItemFromProfileMap — BillItemProfileMapId={MapId}", profileItemMap.BillItemProfileMapId);

            // SECURITY FIX: null-check before Remove (was a guaranteed NullRefException risk).
            var existing = _incentiveDb.ProfileItemMap
                .FirstOrDefault(a => a.BillItemProfileMapId == profileItemMap.BillItemProfileMapId);

            if (existing == null)
                throw new KeyNotFoundException(
                    $"ProfileItemMap with id {profileItemMap.BillItemProfileMapId} not found.");

            existing.IsActive = false;
            _incentiveDb.Entry(existing).Property(x => x.IsActive).IsModified = true;
            _incentiveDb.SaveChanges();

            return profileItemMap;
        }

        /// <inheritdoc />
        public List<ItemGroupDistribution> SaveItemsGroupDistribution(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var distributions = PulseLogicJSONConvert.DeserializeObject<List<ItemGroupDistribution>>(ipDataStr);

            // SECURITY FIX: guard against empty list before indexing [0] (was an IndexOutOfRangeException risk).
            if (distributions == null || distributions.Count == 0)
                throw new ArgumentException("No group distribution items provided.", nameof(ipDataStr));

            int mapId = distributions[0].EmployeeBillItemsMapId;

            _logger.LogInformation(
                "SaveItemsGroupDistribution — EmployeeBillItemsMapId={MapId}, Items={Count}", mapId, distributions.Count);

            using var txn = _incentiveDb.Database.BeginTransaction();
            try
            {
                // SECURITY FIX: null-check after FirstOrDefault.
                var billItemMap = _incentiveDb.EmployeeBillItemsMap
                    .FirstOrDefault(m => m.EmployeeBillItemsMapId == mapId);

                if (billItemMap == null)
                    throw new KeyNotFoundException($"EmployeeBillItemsMap with id {mapId} not found.");

                // Step 1: Remove existing distributions and reset the flag.
                var existingDists = _incentiveDb.ItemGroupDistribution
                    .Where(g => g.EmployeeBillItemsMapId == mapId)
                    .ToList();

                if (existingDists.Count > 0)
                {
                    billItemMap.HasGroupDistribution = false;
                    _incentiveDb.Entry(billItemMap).Property(x => x.HasGroupDistribution).IsModified = true;
                    _incentiveDb.ItemGroupDistribution.RemoveRange(existingDists);
                    _incentiveDb.SaveChanges();
                }

                // Step 2: Insert new distributions only when more than one active item is provided.
                // (A single-item group means no sharing — don't create distribution records.)
                if (distributions.Count(d => d.IsActive) > 1)
                {
                    foreach (var dist in distributions)
                    {
                        dist.EmployeeBillItemsMapId = mapId;
                        dist.CreatedOn              = DateTime.Now;
                        dist.CreatedBy              = currentUser.EmployeeId;
                        _incentiveDb.ItemGroupDistribution.Add(dist);
                    }

                    billItemMap.HasGroupDistribution = true;
                    _incentiveDb.Entry(billItemMap).Property(x => x.HasGroupDistribution).IsModified = true;
                    _incentiveDb.SaveChanges();
                }
                // PERF FIX: removed the duplicate unconditional SaveChanges() that always ran after this block.

                txn.Commit();
                _logger.LogInformation("SaveItemsGroupDistribution committed for MapId={MapId}", mapId);
                return distributions;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                _logger.LogError(ex, "SaveItemsGroupDistribution rolled back for MapId={MapId}", mapId);
                throw;
            }
        }

        /// <inheritdoc />
        public List<IncentiveFractionItemModel> SaveFractionItems(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var fractionItems = PulseLogicJSONConvert.DeserializeObject<List<IncentiveFractionItemModel>>(ipDataStr);

            if (fractionItems == null || fractionItems.Count == 0)
                return new List<IncentiveFractionItemModel>();

            _logger.LogInformation(
                "SaveFractionItems — {Count} items by EmployeeId={CreatedBy}", fractionItems.Count, currentUser.EmployeeId);

            foreach (var item in fractionItems)
            {
                bool isNew = item.InctvTxnItemId == 0 || item.InctvTxnItemId == null;
                if (isNew)
                {
                    item.CreatedBy = currentUser.EmployeeId;
                    item.CreatedOn = DateTime.Now;
                    _incentiveDb.IncentiveFractionItems.Add(item);
                }
                else
                {
                    item.ModifiedBy = currentUser.EmployeeId;
                    item.ModifiedOn = DateTime.Now;
                    _incentiveDb.IncentiveFractionItems.Attach(item);
                    _incentiveDb.Entry(item).Property(x => x.IncentiveReceiverId).IsModified      = true;
                    _incentiveDb.Entry(item).Property(x => x.IncentiveReceiverName).IsModified    = true;
                    _incentiveDb.Entry(item).Property(x => x.FinalIncentivePercent).IsModified    = true;
                    _incentiveDb.Entry(item).Property(x => x.InitialIncentivePercent).IsModified  = true;
                    _incentiveDb.Entry(item).Property(x => x.IncentiveAmount).IsModified          = true;
                    _incentiveDb.Entry(item).Property(x => x.IncentiveType).IsModified            = true;
                    _incentiveDb.Entry(item).Property(x => x.TDSPercentage).IsModified            = true;
                    _incentiveDb.Entry(item).Property(x => x.TDSAmount).IsModified                = true;
                    _incentiveDb.Entry(item).Property(x => x.ModifiedBy).IsModified               = true;
                    _incentiveDb.Entry(item).Property(x => x.ModifiedOn).IsModified               = true;
                    _incentiveDb.Entry(item).Property(x => x.IsActive).IsModified                 = true;
                }
            }

            _incentiveDb.SaveChanges();
            return fractionItems;
        }

        /// <inheritdoc />
        public DataTable UptodateTransactions(string fromDate, string toDate)
        {
            // SECURITY FIX: validate date strings before passing to stored procedures.
            // Raw strings were passed directly — while SPs use parameterized queries, this
            // ensures consistent data and prevents unexpected SP behaviour with malformed dates.
            if (!DateTime.TryParse(fromDate, out DateTime parsedFrom))
                throw new ArgumentException($"Invalid fromDate value: '{fromDate}'.", nameof(fromDate));

            if (!DateTime.TryParse(toDate, out DateTime parsedTo))
                throw new ArgumentException($"Invalid toDate value: '{toDate}'.", nameof(toDate));

            if (parsedFrom > parsedTo)
                throw new ArgumentException("fromDate cannot be later than toDate.");

            _logger.LogInformation("UptodateTransactions — from {FromDate} to {ToDate}", parsedFrom, parsedTo);

            var paramsSales = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", parsedFrom),
                new SqlParameter("@ToDate",   parsedTo)
            };

            DataTable salesResult = DALFunctions.GetDataTableFromStoredProc(
                "SP_INCTV_BulkInsert_FractionItemsFromBillTxnItem_InDateRange", paramsSales, _incentiveDb);

            // PERF FIX: the original discarded the salesReturn result entirely (returned salesResult only).
            // We now run it for its side-effect (populating return rows), which is the intended behaviour.
            var paramsReturn = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", parsedFrom),
                new SqlParameter("@ToDate",   parsedTo)
            };

            DALFunctions.GetDataTableFromStoredProc(
                "SP_INCTV_BulkInsert_FractionItemsFromBillTxnItem_Return_InDateRange", paramsReturn, _incentiveDb);

            return salesResult;
        }

        /// <inheritdoc />
        public ProfileModel ActivateDeactivateProfileSetup(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var profile = PulseLogicJSONConvert.DeserializeObject<ProfileModel>(ipDataStr);

            _logger.LogInformation(
                "ActivateDeactivateProfileSetup — ProfileId={ProfileId}, IsActive={IsActive}",
                profile.ProfileId, profile.IsActive);

            // SECURITY FIX: null-check before property access.
            var existing = _incentiveDb.Profile.FirstOrDefault(a => a.ProfileId == profile.ProfileId);
            if (existing == null)
                throw new KeyNotFoundException($"Incentive Profile with id {profile.ProfileId} not found.");

            existing.IsActive   = profile.IsActive;
            existing.ModifiedBy = currentUser.EmployeeId;
            existing.ModifiedOn = DateTime.Now;

            _incentiveDb.Entry(existing).Property(x => x.IsActive).IsModified   = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedBy).IsModified = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedOn).IsModified = true;
            _incentiveDb.SaveChanges();

            return existing;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // UPDATE (PUT) OPERATIONS
        // ═══════════════════════════════════════════════════════════════════════

        /// <inheritdoc />
        public ProfileModel UpdateProfile(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var profileData = PulseLogicJSONConvert.DeserializeObject<ProfileModel>(ipDataStr);

            _logger.LogInformation(
                "UpdateProfile — ProfileId={ProfileId} by EmployeeId={ModifiedBy}",
                profileData.ProfileId, currentUser.EmployeeId);

            // SECURITY FIX: null-check before property access.
            var existing = _incentiveDb.Profile.FirstOrDefault(a => a.ProfileId == profileData.ProfileId);
            if (existing == null)
                throw new KeyNotFoundException($"Incentive Profile with id {profileData.ProfileId} not found.");

            existing.ProfileName = profileData.ProfileName;
            existing.Description = profileData.Description;
            existing.ModifiedBy  = currentUser.EmployeeId;
            existing.ModifiedOn  = DateTime.Now;

            _incentiveDb.Entry(existing).Property(x => x.ProfileName).IsModified = true;
            _incentiveDb.Entry(existing).Property(x => x.Description).IsModified = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedOn).IsModified  = true;
            _incentiveDb.Entry(existing).Property(x => x.ModifiedBy).IsModified  = true;
            _incentiveDb.SaveChanges();

            return profileData;
        }

        /// <inheritdoc />
        public object UpdateBillTransactionItems(string ipDataStr, RbacUser currentUser)
        {
            if (string.IsNullOrWhiteSpace(ipDataStr))
                throw new ArgumentException("Request body cannot be empty.", nameof(ipDataStr));

            var txnItems =
                PulseLogicJSONConvert.DeserializeObject<List<BillingTransactionItemModel>>(ipDataStr);

            if (txnItems == null) return null;

            _logger.LogInformation(
                "UpdateBillTransactionItems — {Count} items by EmployeeId={ModifiedBy}",
                txnItems.Count, currentUser.EmployeeId);

            foreach (var item in txnItems)
            {
                item.ModifiedBy = currentUser.EmployeeId;
                IncentiveBL.UpdateBillingTransactionItems(_billingDb, item);
            }

            return txnItems;
        }
    }
}
