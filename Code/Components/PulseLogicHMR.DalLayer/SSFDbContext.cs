using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.PatientModels;
using PulseLogicHMR.ServerModel.SSFModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseLogicHMR.DalLayer
{
    public class SSFDbContext : DbContext
    {
        public DbSet<AdminParametersModel> AdminParameters { get; set; }
        public DbSet<SSFClaimResponseDetails> SSFClaimResponseDetail { get; set; }
        public DbSet<PatientSchemeMapModel> PatientSchemeMaps { get; set; }
        public DbSet<VisitModel> Visits { get; set; }
        public DbSet<SSFClaimBookingModel> SSFClaimBookings { get; set; }
        private readonly string _connString;

        public SSFDbContext(string conn)
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminParametersModel>().ToTable("CORE_CFG_Parameters");
            modelBuilder.Entity<SSFClaimResponseDetails>().ToTable("PAT_SSFClaimResponseDetails");
            modelBuilder.Entity<PatientSchemeMapModel>().ToTable("PAT_MAP_PatientSchemes");
            modelBuilder.Entity<VisitModel>().ToTable("PAT_PatientVisits");
            modelBuilder.Entity<SSFClaimBookingModel>().ToTable("PAT_SSF_ClaimBooking");
        }
    }
}

