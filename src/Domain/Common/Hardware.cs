using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Domain.Common
{
    //https://github.com/dotnet/efcore/issues/24614  <- cant have 2 same owned types  so i made 2 base types of hardware to fix this problem. (ugly)


    public class Hardware : ValueObject
    {

        private int _memory;
        private int _storage;
        private int _amountVCPU;



        public int Memory
        {
            get { return _memory; }
            set { _memory = Guard.Against.Negative(value, nameof(_memory)); }
        }
        public int Storage
        {
            get { return _storage; }
            set { _storage = Guard.Against.Negative(value, nameof(_storage)); }
        }
        public int Amount_vCPU
        {
            get { return _amountVCPU; }
            set { _amountVCPU = Guard.Against.Negative(value, nameof(_amountVCPU)); }
        }



        public Hardware(int memory, int storage, int amount_vCPU)
        {
            this.Memory = memory;
            this.Storage = storage;
            this.Amount_vCPU = amount_vCPU;
        }


        public override string ToString() => $"{Amount_vCPU} {Storage} {Memory}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _memory;
            yield return _storage;
            yield return _amountVCPU;
        }
    }
}