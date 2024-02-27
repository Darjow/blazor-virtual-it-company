using Domain.VirtualMachines.VirtualMachine;
using Domain.Server;
using Domain.VirtualMachines.Contract;
using Microsoft.EntityFrameworkCore;
using Domain.Statistics;
using Domain.Projecten;
using Domain.Users;
using System.Reflection;
using Persistence.Data.Configuration;
using Domain;
using Domain.VirtualMachines.BackUp;
using Domain.Common;

namespace Persistence.Data
{
    public class DotNetDbContext : DbContext
    {
        public DbSet<VirtualMachine> VirtualMachines { get; set; }
        public DbSet<FysiekeServer> FysiekeServers { get; set; }
        public DbSet<VMContract> VMContracts { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Administrator> Admins { get; set; }
        public DbSet<InterneKlant> InterneKlanten { get; set; }
        public DbSet<ExterneKlant> ExterneKlanten { get; set; }
        public DbSet<Project> Projecten { get; set; }
        public DbSet<Backup> BackUps { get; set; }
        public DbSet<VMConnection> Connections { get; set; }

        public DotNetDbContext(DbContextOptions<DotNetDbContext> options) : base(options) { }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);  

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          
        }
    }
}

