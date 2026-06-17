using PulseLogicHMR.ServerModel.BillingModels;
using PulseLogicHMR.ServerModel.ClaimManagementModels;
using PulseLogicHMR.ServerModel.PharmacyModels;
using PulseLogicHMR.ServerModel;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseLogicHMR.ServerModel.Utilities;
using PulseLogicHMR.ServerModel.PatientModels;
using PulseLogicHMR.ServerModel.MasterModels;

namespace PulseLogicHMR.DalLayer
{
    public class UtilitiesDbContext : DbContext
    {
        private readonly string _connString;

        public UtilitiesDbContext(string conn)
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

        public DbSet<SchemeRefundModel> SchemeRefunds { get; set; }
        public DbSet<BillingFiscalYear> FiscicalYear { get; set; }
        public DbSet<VisitSchemeChangeHistoryModel> VisitSchemeChangeHistory { get; set; }
        public DbSet<VisitModel> PatientVisitModel { get; set; }
        public DbSet<PatientSchemeMapModel> PatientSchemeMapModel { get; set; }
        public DbSet<BillMapPriceCategorySchemeModel> MapPriceCategoryScheme { get; set; }
        public DbSet<BillingDepositModel> BillingDepositModel { get; set; }

        public DbSet<PaymentModes> PaymentModes { get; set; }
        public DbSet<EmpCashTransactionModel> EmpCashTransactionModels { get; set; }
        public DbSet<CreditOrganizationModel> CreditOrganizationModels { get; set; }
        public DbSet<ProcessConfirmationAuthorityModel> ProcessConfirmationAuthorities { get; set; }

        public DbSet<BillingSchemeModel> BillingSchemeModels { get; set; }
        public DbSet<EmployeeModel> EmployeeModels { get; set; }
        public DbSet<PatientModel> Patient { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchemeRefundModel>().ToTable("BIL_TXN_SchemeRefund");
            modelBuilder.Entity<BillingFiscalYear>().ToTable("BIL_CFG_FiscalYears");
            modelBuilder.Entity<VisitSchemeChangeHistoryModel>().ToTable("VIS_LOG_VisitSchemeChangeHistory");
            modelBuilder.Entity<VisitModel>().ToTable("PAT_PatientVisits");
            modelBuilder.Entity<PatientSchemeMapModel>().ToTable("PAT_MAP_PatientSchemes");
            modelBuilder.Entity<BillMapPriceCategorySchemeModel>().ToTable("BIL_MAP_PriceCategoryVsScheme");
            modelBuilder.Entity<BillingDepositModel>().ToTable("BIL_TXN_Deposit");
            modelBuilder.Entity<PaymentModes>().ToTable("MST_PaymentModes");
            modelBuilder.Entity<EmpCashTransactionModel>().ToTable("TXN_EmpCashTransaction");
            modelBuilder.Entity<CreditOrganizationModel>().ToTable("BIL_MST_Credit_Organization");
            modelBuilder.Entity<BillingSchemeModel>().ToTable("BIL_CFG_Scheme");
            modelBuilder.Entity<EmployeeModel>().ToTable("EMP_Employee");
            modelBuilder.Entity<PatientModel>().ToTable("PAT_Patient");



            modelBuilder.Entity<ProcessConfirmationAuthorityModel>().ToTable("UTL_CFG_ProcessConfirmationAuthority");


        }
    }

}

