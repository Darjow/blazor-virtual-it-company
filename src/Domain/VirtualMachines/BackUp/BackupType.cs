using Domain.VirtualMachines.BackUp;
using Domain.VirtualMachines.VirtualMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.VirtualMachines.BackUp
{
    public enum BackUpType
    {
        GEEN = -1,
        CUSTOM = 1,
        DAILY,
        WEEKLY,
        MONTHLY,
    }
}


public static class Format
{
    public static String GetString(this BackUpType type)
    {
        switch (type)
        {
            case BackUpType.CUSTOM:
                return "Custom";
            case BackUpType.DAILY:
                return "Dagelijks";
            case BackUpType.WEEKLY:
                return "Wekelijks";
            case BackUpType.MONTHLY:
                return "Maandelijks";

            default: throw new ArgumentException("No Case for: " + type.ToString());

        }
    }
}