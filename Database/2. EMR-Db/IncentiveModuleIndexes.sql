-- =============================================================================
-- PulseLogicHMR — Incentive Module: Recommended Database Indexes
-- =============================================================================
-- Author  : Engineering Overhaul (Task 6 — Database Structure)
-- Created : 2026-06-14
-- Purpose : Add missing indexes identified during the code review of
--           IncentiveController.cs / IncentiveService.cs.
--
-- Analysis:
--   The IncentiveService executes several high-frequency queries that filter,
--   join, or scan on columns that currently have no index. Without indexes,
--   SQL Server performs full table scans — O(N) per query.
--   The indexes below convert the most critical scans to O(log N) seeks.
--
-- NOTES:
--   1. Run in a maintenance window or during low traffic.
--   2. All indexes use CREATE INDEX ... IF NOT EXISTS pattern (SQL Server 2016+).
--   3. Review the INCLUDE columns if query plans show key lookups.
--   4. After creation, run: UPDATE STATISTICS <table>; for each affected table.
-- =============================================================================

USE [DEV_PulseLogicHMR_INT];
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_MAP_EmployeeBillItemsMap
--
-- Issue : ActivateDeactivateEmployee, GetEmployeeBillItems, RemoveSelectedBillItem
--         all filter on EmployeeId with WHERE and JOIN — no index exists.
-- Impact: Without this index, every deactivation scans the ENTIRE map table.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_EmpBillMap_EmployeeId'
      AND object_id = OBJECT_ID('INCTV_MAP_EmployeeBillItemsMap')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_EmpBillMap_EmployeeId
        ON INCTV_MAP_EmployeeBillItemsMap (EmployeeId)
        INCLUDE (IsActive, HasGroupDistribution, ServiceItemId, PriceCategoryId,
                 PerformerPercent, PrescriberPercent, ReferrerPercent,
                 BillingTypesApplicable, ModifiedBy, ModifiedOn);

    PRINT 'Created index IX_EmpBillMap_EmployeeId on INCTV_MAP_EmployeeBillItemsMap';
END
ELSE
    PRINT 'Index IX_EmpBillMap_EmployeeId already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_MAP_EmployeeBillItemsMap
--
-- Issue : ServiceItemId is used as a JOIN key against BIL_MST_ServiceItem.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_EmpBillMap_ServiceItemId'
      AND object_id = OBJECT_ID('INCTV_MAP_EmployeeBillItemsMap')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_EmpBillMap_ServiceItemId
        ON INCTV_MAP_EmployeeBillItemsMap (ServiceItemId)
        INCLUDE (EmployeeId, IsActive, PriceCategoryId);

    PRINT 'Created index IX_EmpBillMap_ServiceItemId on INCTV_MAP_EmployeeBillItemsMap';
END
ELSE
    PRINT 'Index IX_EmpBillMap_ServiceItemId already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_CFG_ItemGroupDistribution
--
-- Issue : GetEmployeeBillItems pre-loads all group distributions for a set of
--         EmployeeBillItemsMapIds. SaveItemsGroupDistribution, ActivateDeactivate,
--         and RemoveSelectedBillItem all filter on EmployeeBillItemsMapId.
--         Without an index this is a full table scan on every incentive write.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_GrpDist_EmpBillItemsMapId'
      AND object_id = OBJECT_ID('INCTV_CFG_ItemGroupDistribution')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_GrpDist_EmpBillItemsMapId
        ON INCTV_CFG_ItemGroupDistribution (EmployeeBillItemsMapId)
        INCLUDE (IsActive, DistributeToEmployeeId, DistributionPercent,
                 FixedDistributionAmount, IncentiveType, Remarks);

    PRINT 'Created index IX_GrpDist_EmpBillItemsMapId on INCTV_CFG_ItemGroupDistribution';
END
ELSE
    PRINT 'Index IX_GrpDist_EmpBillItemsMapId already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_BillItems_Profile_Map
--
-- Issue : GetProfileItemsMapping filters on ProfileId — full table scan.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_ProfileItemMap_ProfileId'
      AND object_id = OBJECT_ID('INCTV_BillItems_Profile_Map')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_ProfileItemMap_ProfileId
        ON INCTV_BillItems_Profile_Map (ProfileId)
        INCLUDE (ServiceItemId, PriceCategoryId, IsActive,
                 PerformerPercent, PrescriberPercent, ReferrerPercent,
                 BillingTypesApplicable);

    PRINT 'Created index IX_ProfileItemMap_ProfileId on INCTV_BillItems_Profile_Map';
END
ELSE
    PRINT 'Index IX_ProfileItemMap_ProfileId already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_TXN_IncentiveFractionItem
--
-- Issue : SaveFractionItems, UptodateTransactions, and GetFractionItems SP
--         all filter on BillingTransactionItemId or PaymentInfoId.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_IncentiveFraction_BillingTxnItemId'
      AND object_id = OBJECT_ID('INCTV_TXN_IncentiveFractionItem')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_IncentiveFraction_BillingTxnItemId
        ON INCTV_TXN_IncentiveFractionItem (BillingTransactionItemId)
        INCLUDE (IncentiveReceiverId, IsPaymentProcessed, PaymentInfoId, IsActive);

    PRINT 'Created index IX_IncentiveFraction_BillingTxnItemId on INCTV_TXN_IncentiveFractionItem';
END
ELSE
    PRINT 'Index IX_IncentiveFraction_BillingTxnItemId already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_TXN_IncentiveFractionItem
--
-- Issue : Payment processing filters on IsPaymentProcessed = 0 (pending rows).
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_IncentiveFraction_PaymentPending'
      AND object_id = OBJECT_ID('INCTV_TXN_IncentiveFractionItem')
)
BEGIN
    -- Filtered index — only indexes the subset of unpaid rows; far smaller than full index.
    CREATE NONCLUSTERED INDEX IX_IncentiveFraction_PaymentPending
        ON INCTV_TXN_IncentiveFractionItem (IncentiveReceiverId, TransactionDate)
        INCLUDE (IncentiveAmount, TDSAmount, TDSPercentage)
        WHERE IsPaymentProcessed = 0 AND IsActive = 1;

    PRINT 'Created filtered index IX_IncentiveFraction_PaymentPending on INCTV_TXN_IncentiveFractionItem';
END
ELSE
    PRINT 'Filtered index IX_IncentiveFraction_PaymentPending already exists — skipped.';
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- Table: INCTV_EmployeeIncentiveInfo
--
-- Issue : Every read/write operation lookups by EmployeeId — only PK on InfoId.
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_EmpIncentiveInfo_EmployeeId'
      AND object_id = OBJECT_ID('INCTV_EmployeeIncentiveInfo')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_EmpIncentiveInfo_EmployeeId
        ON INCTV_EmployeeIncentiveInfo (EmployeeId)
        INCLUDE (IsActive, TDSPercent);

    PRINT 'Created unique index IX_EmpIncentiveInfo_EmployeeId on INCTV_EmployeeIncentiveInfo';
END
ELSE
    PRINT 'Index IX_EmpIncentiveInfo_EmployeeId already exists — skipped.';
GO

-- =============================================================================
-- Post-creation: update statistics for query planner accuracy
-- =============================================================================
UPDATE STATISTICS INCTV_MAP_EmployeeBillItemsMap;
UPDATE STATISTICS INCTV_CFG_ItemGroupDistribution;
UPDATE STATISTICS INCTV_BillItems_Profile_Map;
UPDATE STATISTICS INCTV_TXN_IncentiveFractionItem;
UPDATE STATISTICS INCTV_EmployeeIncentiveInfo;

PRINT '=== Incentive module indexes created/verified successfully ===';
GO
