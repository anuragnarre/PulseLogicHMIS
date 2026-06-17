using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.MarketingReferralModel;
using Microsoft.EntityFrameworkCore;

namespace PulseLogicHMR.DalLayer
{
    public class MarketingReferralDbContext : DbContext
    {
        private readonly string _connString;

        public MarketingReferralDbContext(string conn)
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
        public DbSet<BillingTransactionItemModel> BillingTransactionItem { get; set; }
        public DbSet<ReferralSchemeModel> ReferralScheme { get; set; }
        public DbSet<ReferringPartyModel> ReferringParty { get; set; }
        public DbSet<ReferringPartyGroupModel> ReferringPartyGroup { get; set; }
        public DbSet<ReferringOrganizationModel> ReferringOrganization { get; set; }
        public DbSet<ReferralComissionModel> ReferralComission { get; set; }
        public DbSet<BillingFiscalYear> BillingFiscalYears { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingTransactionItemModel>().ToTable("BIL_TXN_BillingTransactionItems");
            modelBuilder.Entity<ReferralSchemeModel>().ToTable("MKT_MST_ReferralScheme");
            modelBuilder.Entity<ReferringPartyModel>().ToTable("MKT_CFG_ReferringParty");
            modelBuilder.Entity<ReferringPartyGroupModel>().ToTable("MKT_MST_ReferringPartyGroup");
            modelBuilder.Entity<ReferringOrganizationModel>().ToTable("MKT_MST_ReferringOrganization");
            modelBuilder.Entity<ReferralComissionModel>().ToTable("MKT_TXN_ReferralCommission");
            modelBuilder.Entity<BillingFiscalYear>().ToTable("BIL_CFG_FiscalYears");
        }
    }
}

