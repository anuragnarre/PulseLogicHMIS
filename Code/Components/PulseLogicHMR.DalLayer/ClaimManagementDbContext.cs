using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.BillingModels;
using PulseLogicHMR.ServerModel.BillingModels.POS;
using PulseLogicHMR.ServerModel.ClaimManagementModels;
using PulseLogicHMR.ServerModel.PharmacyModels;
using Microsoft.EntityFrameworkCore;

namespace PulseLogicHMR.DalLayer
{
    public class ClaimManagementDbContext : DbContext
    {
        private readonly string _connString;

        public ClaimManagementDbContext(string conn)
        {
            _connString = conn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connString))
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        public DbSet<CreditOrganizationModel> CreditOrganization { get; set; }
        public DbSet<BillingTransactionCreditBillStatusModel> BillingCreditBillStatus { get; set; }
        public DbSet<PHRMTransactionCreditBillStatus> PharmacyCreditBillStatus { get; set; }
        public DbSet<InsuranceClaim> InsuranceClaim { get; set; }
        public DbSet<TXNUploadedFile> TXNUploadedFile { get; set; }
        public DbSet<AdminParametersModel> CoreCfgParameter { get; set; }
        public DbSet<InsuranceClaimPayment> InsuranceClaimPayment { get; set; }
        public DbSet<BillInvoiceReturnModel> BILLInvoiceReturn { get; set; }
        public DbSet<PHRMInvoiceReturnModel> PHRMInvoiceReturn { get; set; }
        public DbSet<BillingTransactionItemModel> BillTxnItem { get; set; }
        public DbSet<PHRMInvoiceTransactionItemsModel> PHRMInvoiceTransactionItems { get; set; }
        public DbSet<BillingTransactionCreditBillItemStatusModel> BillingCreditBillItemStatus { get; set; }
        public DbSet<PHRMTransactionCreditBillItemStatusModel> PharmacyCreditBillItemStatus { get; set; }
        public DbSet<BillingSchemeModel> Schemes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreditOrganizationModel>().ToTable("BIL_MST_Credit_Organization");
            modelBuilder.Entity<BillingTransactionCreditBillStatusModel>().ToTable("BIL_TXN_CreditBillStatus");
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>().ToTable("PHRM_TXN_CreditBillStatus");
            modelBuilder.Entity<InsuranceClaim>().ToTable("INS_TXN_InsuranceClaim");
            modelBuilder.Entity<TXNUploadedFile>().ToTable("TXN_UploadedFile");
            modelBuilder.Entity<AdminParametersModel>().ToTable("CORE_CFG_Parameters");
            modelBuilder.Entity<InsuranceClaimPayment>().ToTable("INS_TXN_ClaimPayment");
            modelBuilder.Entity<BillInvoiceReturnModel>().ToTable("BIL_TXN_InvoiceReturn");
            modelBuilder.Entity<PHRMInvoiceReturnModel>().ToTable("PHRM_TXN_InvoiceReturn");
            modelBuilder.Entity<BillingTransactionItemModel>().ToTable("BIL_TXN_BillingTransactionItems");
            modelBuilder.Entity<PHRMInvoiceTransactionItemsModel>().ToTable("PHRM_TXN_InvoiceItems");
            modelBuilder.Entity<BillingTransactionCreditBillItemStatusModel>().ToTable("BIL_TXN_CreditBillItemStatus");
            modelBuilder.Entity<PHRMTransactionCreditBillItemStatusModel>().ToTable("PHRM_TXN_CreditBillItemStatus");
            modelBuilder.Entity<BillingSchemeModel>().ToTable("BIL_CFG_Scheme");

            //setting decimal datatype with precision : 16 and scale : 4
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.NetReceivableAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.NonClaimableAmount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<PHRMTransactionCreditBillItemStatusModel>()
                .Property(p => p.NetTotalAmount)
                .HasPrecision(16,4);

            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.TotalBillAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.ClaimableAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.NonClaimableAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.ClaimedAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.ApprovedAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<InsuranceClaim>()
                .Property(p => p.RejectedAmount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.SalesTotalBillAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.ReturnTotalBillAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.CoPayReceivedAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.CoPayReturnAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.NetReceivableAmount)
                .HasPrecision(16, 4);
            modelBuilder.Entity<PHRMTransactionCreditBillStatus>()
                .Property(p => p.NonClaimableAmount)
                .HasPrecision(16, 4);
        }
    }

}

