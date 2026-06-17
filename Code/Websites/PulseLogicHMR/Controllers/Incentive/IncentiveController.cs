using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Enums;
using PulseLogicHMR.Security;
using PulseLogicHMR.Services.Incentive;
using PulseLogicHMR.Utilities;

namespace PulseLogicHMR.Controllers
{
    /// <summary>
    /// Incentive module API controller.
    ///
    /// Responsibilities (ONLY):
    ///   1. Route HTTP requests to the correct service method.
    ///   2. Extract the current user from session.
    ///   3. Delegate ALL business logic to <see cref="IIncentiveService"/>.
    ///   4. Return the result via the framework's InvokeHttp* helpers.
    ///
    /// The previous 1,937-line "God Class" has been split:
    ///   - Business logic → IncentiveService (Tasks 1–3)
    ///   - Dead reqType legacy block (lines 888–1933) → DELETED
    ///
    /// This controller is intentionally kept thin (~130 lines).
    /// </summary>
    public class IncentiveController : CommonController
    {
        private readonly IIncentiveService _incentiveService;

        /// <summary>
        /// Constructor-injection via DI — DbContexts are no longer instantiated with `new` here.
        /// Register <see cref="IIncentiveService"/> / <see cref="IncentiveService"/> in
        /// <c>DependencyInjection/PulseLogicServiceRegistration.cs</c> (or equivalent).
        /// </summary>
        public IncentiveController(
            IOptions<MyConfiguration> config,
            IIncentiveService incentiveService)
            : base(config)
        {
            _incentiveService = incentiveService
                ?? throw new ArgumentNullException(nameof(incentiveService));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GET
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet, Route("Profiles")]
        public IActionResult Profiles() =>
            InvokeHttpGetFunction(() => _incentiveService.GetProfiles());

        [HttpGet, Route("Categories")]
        public IActionResult Categories() =>
            InvokeHttpGetFunction(() => _incentiveService.GetCategories());

        [HttpGet, Route("ProfileItems")]
        public IActionResult ProfileItems() =>
            InvokeHttpGetFunction(() => _incentiveService.GetProfileItems());

        [HttpGet, Route("ProfileItemsMapping")]
        public IActionResult ProfileItemsMapping(int profileId) =>
            InvokeHttpGetFunction(() => _incentiveService.GetProfileItemsMapping(profileId));

        [HttpGet, Route("EmployeesIncentiveInfo")]
        public IActionResult EmployeesIncentiveInfo() =>
            InvokeHttpGetFunction(() => _incentiveService.GetEmployeesIncentiveInfo());

        [HttpGet, Route("IncentiveItems")]
        public IActionResult IncentiveItems(int priceCategoryId) =>
            InvokeHttpGetFunction(() => _incentiveService.GetIncentiveItems(priceCategoryId));

        [HttpGet, Route("EmployeeBillItems")]
        public IActionResult EmployeeBillItems(int employeeId) =>
            InvokeHttpGetFunction(() => _incentiveService.GetEmployeeBillItems(employeeId));

        [HttpGet, Route("TransactionItems")]
        public IActionResult TransactionItems(DateTime fromDate, DateTime toDate) =>
            InvokeHttpGetFunction(() => _incentiveService.GetTransactionItems(fromDate, toDate));

        [HttpGet, Route("TransactionInvoices")]
        public IActionResult TransactionInvoices(DateTime fromDate, DateTime toDate, int? employeeId = 0) =>
            InvokeHttpGetFunction(() => _incentiveService.GetTransactionInvoices(fromDate, toDate, employeeId));

        [HttpGet, Route("TransactionInvoiceItems")]
        public IActionResult TransactionInvoiceItems(int BillingTransactionId) =>
            InvokeHttpGetFunction(() => _incentiveService.GetTransactionInvoiceItems(BillingTransactionId));

        [HttpGet, Route("FractionOfBillTransactionItems")]
        public IActionResult FractionOfBillTransactionItems(int billTxnItemId) =>
            InvokeHttpGetFunction(() => _incentiveService.GetFractionOfBillTransactionItems(billTxnItemId));

        [HttpGet, Route("IncentiveApplicableDoctors")]
        public IActionResult IncentiveApplicableDoctors() =>
            InvokeHttpGetFunction(() => _incentiveService.GetIncentiveApplicableDoctors());

        // ═══════════════════════════════════════════════════════════════════════
        // POST
        // ═══════════════════════════════════════════════════════════════════════

        [HttpPost, Route("Profile")]
        public IActionResult AddProfile()
        {
            string ipDataStr       = this.ReadPostData();
            RbacUser currentUser   = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.SaveProfile(ipDataStr, currentUser));
        }

        [HttpPost, Route("MapProfileItems")]
        public IActionResult MapProfileItems()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.SaveProfileItemsMap(ipDataStr, currentUser));
        }

        [HttpPost, Route("MapEmployeeBillItems")]
        public IActionResult MapEmployeeBillItems()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.SaveEmployeeBillItemsMap(ipDataStr, currentUser));
        }

        [HttpPost, Route("ActivateDeactivateEmployeeSetup")]
        public IActionResult ActivateDeactivateEmployeeSetup()
        {
            string ipDataStr = this.ReadPostData();
            return InvokeHttpPostFunction(() => _incentiveService.ActivateDeactivateEmployee(ipDataStr));
        }

        [HttpPost, Route("EmployeeBillItems")]
        public IActionResult EmployeeBillItem()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.UpdateEmployeeBillItem(ipDataStr, currentUser));
        }

        [HttpPost, Route("ProfileBillItemMap")]
        public IActionResult ProfileBillItemMap()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.UpdateProfileBillItemMap(ipDataStr, currentUser));
        }

        [HttpPost, Route("RemoveBillItem")]
        public IActionResult RemoveBillItem()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.RemoveSelectedBillItem(ipDataStr, currentUser));
        }

        [HttpPost, Route("RemoveBillItemFromProfileMap")]
        public IActionResult RemoveBillItemFromProfileMap()
        {
            string ipDataStr = this.ReadPostData();
            return InvokeHttpPostFunction(() => _incentiveService.RemoveSelectedBillItemFromProfileMap(ipDataStr));
        }

        [HttpPost, Route("ItemsGroupDistribution")]
        public IActionResult ItemsGroupDistribution()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.SaveItemsGroupDistribution(ipDataStr, currentUser));
        }

        [HttpPost, Route("FractionItems")]
        public IActionResult FractionItems()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.SaveFractionItems(ipDataStr, currentUser));
        }

        [HttpPost, Route("Transactions")]
        public IActionResult Transactions(string fromDate, string toDate) =>
            InvokeHttpPostFunction(() => _incentiveService.UptodateTransactions(fromDate, toDate));

        [HttpPost, Route("ActivateDeactivateProfile")]
        public IActionResult ActivateDeactivateProfile()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPostFunction(() => _incentiveService.ActivateDeactivateProfileSetup(ipDataStr, currentUser));
        }

        // ═══════════════════════════════════════════════════════════════════════
        // PUT
        // ═══════════════════════════════════════════════════════════════════════

        [HttpPut, Route("Profile")]
        public IActionResult UpdateProfile()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPutFunction(() => _incentiveService.UpdateProfile(ipDataStr, currentUser));
        }

        [HttpPut, Route("BillTransactionItems")]
        public IActionResult BillTransactionItems()
        {
            string ipDataStr     = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            return InvokeHttpPutFunction(() => _incentiveService.UpdateBillTransactionItems(ipDataStr, currentUser));
        }
    }
}
