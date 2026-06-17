using System;
using System.Collections.Generic;
using System.Data;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel.IncentiveModels;

namespace PulseLogicHMR.Services.Incentive
{
    /// <summary>
    /// Defines all business operations for the Incentive module.
    /// Implementations should handle data access, validation, and transaction management.
    /// The controller is responsible only for routing; this interface owns the domain logic.
    /// </summary>
    public interface IIncentiveService
    {
        // ── READ ────────────────────────────────────────────────────────────────

        /// <summary>Returns all incentive profiles joined with their price categories.</summary>
        object GetProfiles();

        /// <summary>Returns all active price categories.</summary>
        object GetCategories();

        /// <summary>Returns all active billing service items eligible for profile mapping.</summary>
        object GetProfileItems();

        /// <summary>Returns the full profile detail including its mapped bill items.</summary>
        /// <param name="profileId">The profile to retrieve. Must be &gt; 0.</param>
        object GetProfileItemsMapping(int profileId);

        /// <summary>Returns all employees that have an incentive info record.</summary>
        object GetEmployeesIncentiveInfo();

        /// <summary>
        /// Returns all active service items applicable for incentive, filtered by price category.
        /// Falls back to the default price category when <paramref name="priceCategoryId"/> is 0.
        /// </summary>
        object GetIncentiveItems(int priceCategoryId);

        /// <summary>
        /// Returns the incentive setup for a single employee, including all mapped bill items
        /// and their group distributions.
        /// </summary>
        /// <param name="employeeId">Must be &gt; 0.</param>
        object GetEmployeeBillItems(int employeeId);

        /// <summary>Runs SP_INCTV_GetBillingTxnItems_BetweenDate for a date range.</summary>
        DataTable GetTransactionItems(DateTime fromDate, DateTime toDate);

        /// <summary>Runs SP_INCTV_ViewTxn_InvoiceLevel for a date range and optional employee.</summary>
        DataTable GetTransactionInvoices(DateTime fromDate, DateTime toDate, int? employeeId);

        /// <summary>Runs SP_INCTV_ViewTxn_InvoiceItemLevel and returns both result tables.</summary>
        object GetTransactionInvoiceItems(int billingTransactionId);

        /// <summary>Runs SP_INCTV_GetFractionItems_ByTxnItemId.</summary>
        DataTable GetFractionOfBillTransactionItems(int billTxnItemId);

        /// <summary>Returns all employees who are active and incentive-applicable.</summary>
        object GetIncentiveApplicableDoctors();

        // ── WRITE ───────────────────────────────────────────────────────────────

        /// <summary>Creates a new incentive profile.</summary>
        ProfileModel SaveProfile(string ipDataStr, RbacUser currentUser);

        /// <summary>Saves the bill-item-to-profile mapping list.</summary>
        object SaveProfileItemsMap(string ipDataStr, RbacUser currentUser);

        /// <summary>
        /// Creates or updates an employee's incentive info record and its bill item mappings.
        /// Runs inside a DB transaction; rolls back on any failure.
        /// </summary>
        EmployeeIncentiveInfo SaveEmployeeBillItemsMap(string ipDataStr, RbacUser currentUser);

        /// <summary>
        /// Activates or deactivates an employee's incentive setup, cascading to all mapped
        /// bill items and their group distributions in a single transaction.
        /// </summary>
        EmployeeIncentiveInfo ActivateDeactivateEmployee(string ipDataStr);

        /// <summary>Updates a single employee bill item mapping (percentages, billing types).</summary>
        EmployeeBillItemsMap UpdateEmployeeBillItem(string ipDataStr, RbacUser currentUser);

        /// <summary>Updates a single profile bill item mapping (percentages, billing types).</summary>
        ProfileItemMap UpdateProfileBillItemMap(string ipDataStr, RbacUser currentUser);

        /// <summary>
        /// Soft-deletes a bill item mapping and permanently removes its group distributions
        /// in one transaction.
        /// </summary>
        EmployeeBillItemsMap RemoveSelectedBillItem(string ipDataStr, RbacUser currentUser);

        /// <summary>Soft-deactivates a bill item from a profile map.</summary>
        ProfileItemMap RemoveSelectedBillItemFromProfileMap(string ipDataStr);

        /// <summary>
        /// Replaces all group distributions for an employee bill item mapping.
        /// Clears existing rows first, then inserts new ones in one transaction.
        /// </summary>
        List<ItemGroupDistribution> SaveItemsGroupDistribution(string ipDataStr, RbacUser currentUser);

        /// <summary>Creates or updates incentive fraction items in bulk.</summary>
        List<IncentiveFractionItemModel> SaveFractionItems(string ipDataStr, RbacUser currentUser);

        /// <summary>
        /// Bulk-loads incentive fraction items from billing transactions within a date range.
        /// </summary>
        /// <param name="fromDate">ISO date string; validated before use.</param>
        /// <param name="toDate">ISO date string; validated before use.</param>
        DataTable UptodateTransactions(string fromDate, string toDate);

        /// <summary>Activates or deactivates an incentive profile.</summary>
        ProfileModel ActivateDeactivateProfileSetup(string ipDataStr, RbacUser currentUser);

        // ── UPDATE (PUT) ─────────────────────────────────────────────────────────

        /// <summary>Updates an existing incentive profile's name and description.</summary>
        ProfileModel UpdateProfile(string ipDataStr, RbacUser currentUser);

        /// <summary>Updates the doctor/prescriber assignments on billing transaction items.</summary>
        object UpdateBillTransactionItems(string ipDataStr, RbacUser currentUser);
    }
}
