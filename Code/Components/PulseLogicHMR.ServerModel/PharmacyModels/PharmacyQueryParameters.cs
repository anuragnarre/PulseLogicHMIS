using System;

namespace PulseLogicHMR.ServerModel.PharmacyModels
{
    public class PharmacyQueryParameters
    {
        public string reqType { get; set; }
        public int? supplierId { get; set; }
        public int itemTypeId { get; set; }
        public int companyId { get; set; }
        public int categoryId { get; set; }
        public string status { get; set; }
        public int purchaseOrderId { get; set; }
        public int goodsReceiptId { get; set; }
        public int itemId { get; set; }
        public string batchNo { get; set; }
        public int returnToSupplierId { get; set; }
        public int invoiceId { get; set; }
        public int writeOffId { get; set; }
        public int employeeId { get; set; }
        public int? patientId { get; set; }
        public int providerId { get; set; }
        public int visitId { get; set; }
        public bool IsOutdoorPat { get; set; }
        public int requisitionId { get; set; }
        public int dispatchId { get; set; }
        public DateTime currentDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FiscalYearId { get; set; }
        public int DispensaryId { get; set; }
        public int settlementId { get; set; }
        public int invoiceid { get; set; }
        public int? gdprintId { get; set; }
        public int invoiceretid { get; set; }
        public string CancelRemarks { get; set; }
        public int storeId { get; set; }
        public string invoiceNo { get; set; }
        public int organizationId { get; set; }
        public int CreditNotePrintId { get; set; }
        public int? PriceCategoryId { get; set; }
        public int? PatientVisitId { get; set; }
        public string MemberNo { get; set; }
    }
}
