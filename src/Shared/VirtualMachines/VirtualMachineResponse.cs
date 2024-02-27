namespace Shared.VirtualMachines
{
    public static class VirtualMachineResponse
    {
        public class GetIndex
        {
            public List<VirtualMachineDto.Index> VirtualMachines { get; set; } = new();
            public int TotalAmount { get; set; }

        }

        public class GetDetail :VirtualMachineDto.Detail
        {
        }

        public class Delete
        {
            public int Id { get; set; }

        }

        public class Create
        {
            public int Id { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
        }

        public class Rapport
        {
            public VirtualMachineDto.Rapportage VirtualMachine { get; set; }
        }


    }
}