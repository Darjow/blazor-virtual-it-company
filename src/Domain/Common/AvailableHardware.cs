using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    //https://github.com/dotnet/efcore/issues/24614  <- cant have 2 same owned types  so i made 2 base types of hardware to fix this problem. (ugly)

    public class AvailableHardware : Hardware
    {
        public AvailableHardware(int memory, int storage, int amount_vCPU) : base(memory, storage, amount_vCPU)
        {
        }
    }
}
