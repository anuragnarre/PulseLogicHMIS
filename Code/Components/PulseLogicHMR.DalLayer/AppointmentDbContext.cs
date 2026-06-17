using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseLogicHMR.ServerModel;

namespace PulseLogicHMR.DalLayer
{
    public class AppointmentDbContext : DbContext
    {
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<VisitModel> Visit { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        private readonly string _connString;

        public AppointmentDbContext(string conn)
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
            modelBuilder.Entity<AppointmentModel>().ToTable("PAT_Appointment");
            modelBuilder.Entity<VisitModel>().ToTable("PAT_PatientVisits");
            modelBuilder.Entity<EmployeeModel>().ToTable("EMP_Employee");

        }

    }
}


