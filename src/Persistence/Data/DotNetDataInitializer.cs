using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Domain.Common;
using Domain.Projecten;
using Domain.Server;
using Domain.Users;
using Domain.Utility;
using Domain.VirtualMachines.Contract;
using Fakers.User;
using Fakers.VirtualMachines;

namespace Persistence.Data
{
    public class DotNetDataInitializer
    {
        private readonly DotNetDbContext _dbContext;


        public DotNetDataInitializer(DotNetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {

                SeedAdmins();
                SeedClients();
                SeedVirtualMachines();
                SeedProjects();
                SeedContracts();
            }
        }

   

        private void SeedProjects()
        {
            var customers = _dbContext.Klanten.ToList();
            var virtualMachines = _dbContext.VirtualMachines.ToList();

            var project1 = new Project("Project X");
            var project2 = new Project("A Project");
            var project3 = new Project("Huge Project");

            project1.Klant = customers[0];
            project2.Klant = customers[1];
            project3.Klant = customers[2];

            project1.VirtualMachines = virtualMachines.Take(5).ToList();
            project2.VirtualMachines = virtualMachines.Skip(5).Take(5).ToList();
            project3.VirtualMachines = virtualMachines.Skip(10).Take(6).ToList();

            _dbContext.Projecten.AddRange(project1, project2, project3);
            _dbContext.SaveChanges();

        }
        private void SeedContracts()
        {
            var projects = _dbContext.Projecten.ToList();
            var contracts = new List<VMContract>();

            foreach(var project in projects)
            {
                var customer = project.Klant;
                var vms = project.VirtualMachines;

                foreach(var vm in vms)
                {
                    var contract = new VMContract(customer.Id, vm.Id, DateTime.Now.Subtract(TimeSpan.FromDays(RandomNumberGenerator.GetInt32(300))), DateTime.Now.AddDays(RandomNumberGenerator.GetInt32(200)));

                    vm.Contract = contract;
                    contracts.Add(contract);
                }

            }
            _dbContext.VMContracts.AddRange(contracts);
            _dbContext.SaveChanges();

        }
        private void SeedVirtualMachines()
        {
            var vms = VirtualMachineFaker.Instance.Generate(16);
            var vmsWithoutContracts = vms.Select(e => { e.Contract = null; return e; });

            var backups = vms.Select(e => e.BackUp);
            _dbContext.BackUps.AddRange(backups);
            _dbContext.SaveChanges();

            var connections = vms.Select(e => e.Connection);
            _dbContext.Connections.AddRange(connections);
            _dbContext.SaveChanges();

            var servers = vms.Select(e => e.FysiekeServer);


            vms.ForEach(e =>
            {
                Hardware hw = e.Hardware;
                string name = e.FysiekeServer.Naam;
                FysiekeServer server = servers.First(e => e.Naam == name);
                server.AddConnection(e);
            });


            _dbContext.FysiekeServers.AddRange(servers);
            _dbContext.SaveChanges();
            
            _dbContext.VirtualMachines.AddRange(vmsWithoutContracts);
            _dbContext.SaveChanges();
        }
        private void SeedAdmins()
        {
            var admins = UserFaker.Administrators.Instance.Generate(2);
            var adminLogin = new Administrator("Darjow", "Darjow", "049711172", "darjow@hotmail.com", PasswordUtilities.HashPassword("Password.1"),
              AdminRole.BEHEREN);
           

            _dbContext.Admins.AddRange(admins);
            _dbContext.Admins.Add(adminLogin);
            _dbContext.SaveChanges();
        }
        private void SeedClients()
        {
            var clients = UserFaker.Klant.Instance.Generate(2);
            var clientLogin = new ExterneKlant("Billson", "Billy", "049732873", "billyBillson1997@gmail.com", PasswordUtilities.HashPassword("Password.1"), "CERM", BedrijfType.VOKA, null, null);

            _dbContext.Klanten.Add(clientLogin);
            _dbContext.Klanten.AddRange(clients);
            _dbContext.SaveChanges();

        }
    }
}