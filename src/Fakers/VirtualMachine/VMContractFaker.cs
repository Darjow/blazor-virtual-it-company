using Bogus;
using Domain.Users;
using Domain.VirtualMachines.Contract;
using Domain.VirtualMachines.VirtualMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fakers.Contract
{
    public class VMContractFaker : Faker<VMContract>
    {
        private List<VMContract> _contracts = new();

        private int id = 1;

        private static VMContractFaker? _instance;
        public static VMContractFaker Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new VMContractFaker();
                }
                return _instance;
            }
        }

        public VMContractFaker()
        {
            RuleFor(e => e.Id, _ => id++);
            CustomInstantiator(e => new VMContract(id, id,  DateTime.Now.Subtract(TimeSpan.FromDays(RandomNumberGenerator.GetInt32(300))), DateTime.Now.AddDays(RandomNumberGenerator.GetInt32(150))));
        }


        public override List<VMContract> Generate(int count, string ruleSets = null)
        {

            List<VMContract> output = new();
            if (_contracts.Count == 0)
            {
                _contracts = base.Generate(count, ruleSets);
                output = _contracts;
            }
            else if (_contracts.Count < count)
            {
                output = base.Generate(count - _contracts.Count());
                output.ForEach(e => _contracts.Add(e));
                output = _contracts.GetRange(0, count);

            }
            else
            {
                output = _contracts.GetRange(0, count);

            }
            return output;
        }


        public VMContract GenerateOne()
        {

            VMContract output;

            if (_contracts.Count == 0)
            {
                _contracts = base.Generate(20);
            }

            output = _contracts[RandomNumberGenerator.GetInt32(0, _contracts.Count)];

            return output;
        }
    }

}