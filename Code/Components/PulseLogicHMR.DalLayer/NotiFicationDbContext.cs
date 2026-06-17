using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.ServerModel.NotificationModels;

namespace PulseLogicHMR.DalLayer
{
    public class NotiFicationDbContext : DbContext
    {
        
        public DbSet<NotificationViewModel> Notifications { get; set; }
        public DbSet<VisitModel> PatientVisits { get; set; }

        private readonly string _connString;

        public NotiFicationDbContext(string conn)
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
            modelBuilder.Entity<NotificationViewModel>().ToTable("CORE_Notification");
            modelBuilder.Entity<VisitModel>().ToTable("PAT_PatientVisits");
        }
    }
}

