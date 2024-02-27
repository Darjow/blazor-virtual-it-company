using Domain.Common;
using Shared.VirtualMachines;

namespace Shared.Servers
{
    public static class FysiekeServerResponse
    {

        public class Available
        {
            public List<FysiekeServerDto.Index> Servers { get; set; }
            public int Count { get; set; }

        }

        public class Details
        {
            public FysiekeServerDto.Detail Server { get; set; }
        }

        public class Launched
        {
            public VirtualMachineDto.Detail VirtualMachine { get; set; } // VirtualMachine heeft een VMConnection gekregen en wordt teruggeven.
        }

        public class ResourcesAvailable
        {
            public List<FysiekeServerDto.Beschikbaarheid> Servers { get; set; }
        }
        public class GraphValues
        {
            public Dictionary<DateTime, Hardware> GraphData { get; set; }
        }
    }
}