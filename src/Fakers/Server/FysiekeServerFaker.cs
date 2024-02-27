using Bogus;
using Domain.Common;
using Domain.VirtualMachines.VirtualMachine;
using Domain.Server;
using Fakers.VirtualMachines;
using System;
using System.Linq;

namespace Fakers.Server
{
    public class FysiekeServerFaker : Faker<FysiekeServer>
    {
        private int id = 1;


        private static FysiekeServerFaker? _instance;

        public static FysiekeServerFaker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FysiekeServerFaker();
                }
                return _instance;
            }
        }
        public FysiekeServerFaker()
        {

            Hardware hw = null;


            CustomInstantiator(e => {
                hw = GenerateRandomHardware();
                return new FysiekeServer("Server " + id, new TotalHardware(hw.Memory, hw.Storage, hw.Amount_vCPU), e.Internet.DomainName() + "." + "mock.be");
            });

            RuleFor(e => e.Id, _ => id++);
            RuleFor(e => e.VirtualMachines, _ => VirtualMachineFaker.Instance.Generate(12));
            RuleFor(e => e.HardwareAvailable, _ => new Hardware(hw.Memory - new Random().Next(1, hw.Memory), hw.Storage - new Random().Next(1, hw.Storage), hw.Amount_vCPU - new Random().Next(1, hw.Amount_vCPU)));

        }


        public static Hardware GenerateRandomHardware()
        {
            int[] _memoryOptions = { 256_000, 312_000 };
            int[] _storageOptions = { 5_000_000, 8_000_000, 10_000_000 };
            int[] _cpus = { 100,120,160,200 };


            return new Hardware(_memoryOptions[new Random().Next(0, _memoryOptions.Count())], _storageOptions[new Random().Next(0, _storageOptions.Count())], _cpus[new Random().Next(0, _cpus.Count())]);
        }
    }
}