using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.VirtualMachines.VirtualMachine
{
    public enum VirtualMachineMode
    {
        WAITING_APPROVEMENT,       // No connection || No server
        READY,                     // has connection && server
        RUNNING,
        PAUSED,
        STOPPED

    }

    public static class Format
    {
        public static String GetString(this VirtualMachineMode mode)
        {
            switch (mode)
            {
                case VirtualMachineMode.WAITING_APPROVEMENT:
                    return "Wachten op goedkeuring";
                case VirtualMachineMode.STOPPED:
                    return "Gestopt";
                case VirtualMachineMode.READY:
                    return "Gereed";
                case VirtualMachineMode.RUNNING:
                    return "Actief";
                case VirtualMachineMode.PAUSED:
                    return "Gepauzeerd";

                default: throw new ArgumentException("No Case for: " + mode.ToString());

            }
        }
        public static String GetDotIcon(this VirtualMachineMode mode)
        {
            switch (mode)
            {
                case VirtualMachineMode.STOPPED:
                    return "dot is-red";
                case VirtualMachineMode.READY:
                    return "dot is-blue";
                case VirtualMachineMode.RUNNING:
                    return "dot is-green";
                case VirtualMachineMode.WAITING_APPROVEMENT:
                    return "dot is-orange";
                case VirtualMachineMode.PAUSED:
                    return "dot is-yellow";

                default:
                    throw new ArgumentException("received invalid virtualmachinemode: " + mode.ToString());
            }
        }
    }



};