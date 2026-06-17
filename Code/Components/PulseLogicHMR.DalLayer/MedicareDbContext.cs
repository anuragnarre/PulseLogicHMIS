using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.AccountingModels;
using PulseLogicHMR.ServerModel.MedicareModels;
using Microsoft.EntityFrameworkCore;

namespace PulseLogicHMR.DalLayer
{
    public class MedicareDbContext: DbContext
    {
        private readonly string _connString;

        public MedicareDbContext(string conn)
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
        public DbSet<MedicareMember> MedicareMembers { get; set; }
        public DbSet<MedicareTypes> MedicareTypes { get; set; }
        public DbSet<MedicareInstitutes> MedicareInstitutes { get; set; }
        public DbSet<MedicareMemberBalance> MedicareMemberBalance { get; set; }
        public DbSet<EmployeeRoleModel> EmployeeRole { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<InsuranceProviderModel> InsuranceProvider { get; set; }
        public DbSet<SubLedgerModel> SubLedger { get; set; }
        public DbSet<HospitalModel> Hospital { get; set; }
        public DbSet<LedgerMappingModel> LedgerMapping { get; set; }
        public DbSet<SubLedgerBalanceHistory> SubLedgerBalanceHistory { get; set; }
        public DbSet<FiscalYearModel> FiscalYears { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicareMember>().ToTable("INS_MedicareMember");
            modelBuilder.Entity<MedicareTypes>().ToTable("INS_MST_MedicareType");
            modelBuilder.Entity<MedicareInstitutes>().ToTable("INS_MST_MedicareInstitute");
            modelBuilder.Entity<MedicareMemberBalance>().ToTable("INS_MedicareMemberBalance");
            modelBuilder.Entity<EmployeeRoleModel>().ToTable("EMP_EmployeeRole");
            modelBuilder.Entity<DepartmentModel>().ToTable("MST_Department");
            modelBuilder.Entity<InsuranceProviderModel>().ToTable("INS_CFG_InsuranceProviders");
            modelBuilder.Entity<SubLedgerModel>().ToTable("ACC_MST_SubLedger");
            modelBuilder.Entity<HospitalModel>().ToTable("ACC_MST_Hospital");
            modelBuilder.Entity<LedgerMappingModel>().ToTable("ACC_Ledger_Mapping");
            modelBuilder.Entity<SubLedgerBalanceHistory>().ToTable("ACC_SubLedgerBalanceHistory");
            modelBuilder.Entity<FiscalYearModel>().ToTable("ACC_MST_FiscalYears");
        }
    }
}

