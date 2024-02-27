using Bogus;
using Domain.Common;
using Domain.Server;
using Domain.Statistics;
using Domain.Utility;
using Domain.VirtualMachines.BackUp;
using Domain.VirtualMachines.Contract;
using Domain.VirtualMachines.VirtualMachine;
using Fakers.Contract;
using Fakers.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Fakers.VirtualMachines
{

    public class VirtualMachineFaker : Faker<VirtualMachine>
    {

        private int id = 1;

        private List<VirtualMachine> _virtualMachines = new();
        private List<FysiekeServer> _servers = new();


        private static VirtualMachineFaker instance = null;

        public static VirtualMachineFaker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VirtualMachineFaker();
                }
                return instance;
            }
        }


        public VirtualMachineFaker()
        {

            Hardware hardware = null;
            VMContract contract = null;
            string password = PasswordUtilities.GeneratePassword(12, 3, 3, 3, 2);
            string hash = PasswordUtilities.HashPassword(password);


            CustomInstantiator(e =>
            {
                hardware = GenerateRandomHardware();
                contract = VMContractFaker.Instance.GenerateOne();

                return new VirtualMachine(
                    e.Commerce.ProductName(),
                    e.PickRandom<OperatingSystemEnum>(),
                    hardware,
                    e.PickRandom(GenerateRandomBackups())
               );

            });
            // RuleFor(x => x.Id, _ => id++);
            RuleFor(x => x.Connection, _ => new VMConnection("MOCK-FQDN", GetRandomIpAddress(), "MOCK-USER", password, hash));
            RuleFor(x => x.Mode, x => x.PickRandom<VirtualMachineMode>());
            RuleFor(x => x.Contract, _ => contract);
            RuleFor(x => x.FysiekeServer, _ => GenerateFysiekeServer());


            RuleFor(x => x.Statistics, _ => new Statistic(contract.StartDate, contract.EndDate, hardware));
        }

        public FysiekeServer GenerateFysiekeServer()
        {
            Dictionary<string, string> mockValues = new Dictionary<string, string>() {
                { "Foo - Bar", "nl2.mjfzjfkejlkmoijfzmlj.bla" },
                { "Some - Where", "sw14.somewhere.anon" },
                { "Yoo - Mattie", "ym1.jkmkjlmkj.be" }
            };

            if (_servers.Count < 3)
            {
                var mockObj = mockValues.ElementAt(_servers.Count);

                Hardware y = FysiekeServerFaker.GenerateRandomHardware();
                _servers.Add(new FysiekeServer(mockObj.Key, new TotalHardware(y.Memory, y.Storage, y.Amount_vCPU), mockObj.Value));
                return _servers.Last();

            }
            return _servers[RandomNumberGenerator.GetInt32(0, 3)];



        }

        public override List<VirtualMachine> Generate(int count, string ruleSets = null)
        {
            List<VirtualMachine> output = new();
            if (_virtualMachines.Count() == 0)
            {
                _virtualMachines = base.Generate(count, ruleSets);
                output = _virtualMachines;
            }
            else if (_virtualMachines.Count < count)
            {
                output = base.Generate(count - _virtualMachines.Count());
                output.ForEach(e => _virtualMachines.Add(e));
                output = _virtualMachines.GetRange(0, count);
            }
            else
            {

                output = _virtualMachines.GetRange(0, count);

            }
            return output;
        }



        private Hardware GenerateRandomHardware()
        {

            int[] _memoryOptions = { 1_000, 2_000, 4_000, 8_000, 16_000 };
            int[] _storageOptions = { 250, 1_000, 2_000, 5_000, 10_000, 20_000, 50_000, 100_000, 200_000, 500_000 };


            return new Hardware(_memoryOptions[new Random().Next(0, _memoryOptions.Count())], _storageOptions[new Random().Next(0, _storageOptions.Count())], new Random().Next(1, 8));
        }

        private Backup GenerateRandomBackups()
        {
            List<Backup> res = new();

            int r = new Random().Next(0, 10);
            Backup a;

            if (r == 1)
                a = new Backup(BackUpType.DAILY, DateTime.Now.Subtract(TimeSpan.FromMinutes(500)));
            else if (r == 2)
                a = new Backup(BackUpType.CUSTOM, DateTime.Now.Subtract(TimeSpan.FromHours(5)));
            else if (r <= 6)
                a = new Backup(BackUpType.WEEKLY, DateTime.Now.Subtract(TimeSpan.FromDays(new Random().NextDouble() * 7)));
            else
                a = new Backup(BackUpType.MONTHLY, DateTime.Now.Subtract(TimeSpan.FromDays(new Random().Next(30))));


            return a;

        }

        private IPAddress GetRandomIpAddress()
        {
            var random = new Random();
            var data = new byte[4];
            random.NextBytes(data);

            return new IPAddress(data);
        }



    }
}