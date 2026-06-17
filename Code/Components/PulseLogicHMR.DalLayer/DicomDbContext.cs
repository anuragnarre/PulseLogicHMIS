using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseLogicHMR.ServerModel;
using Microsoft.EntityFrameworkCore;

namespace PulseLogicHMR.DalLayer
{
    public class DicomDbContext : DbContext
    {
        private readonly string _connString;

        public DicomDbContext(string conn)
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

        public DbSet<PatientStudyModel> PatientStudies { get; set; }
        public DbSet<DicomFileInfoModel> DicomFiles { get; set; }
        public DbSet<SeriesInfoModel> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientStudyModel>().ToTable("DCM_PatientStudy");
            modelBuilder.Entity<DicomFileInfoModel>().ToTable("DCM_DicomFiles");
            modelBuilder.Entity<SeriesInfoModel>().ToTable("DCM_Series");
        }
    }
}

