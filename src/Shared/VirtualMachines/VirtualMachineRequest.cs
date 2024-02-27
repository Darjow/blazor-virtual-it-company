using Domain.VirtualMachines.VirtualMachine;
using System.ComponentModel.DataAnnotations;

namespace Shared.VirtualMachines
{
    public static class VirtualMachineRequest
    {
        public class GetIndex
        {
            [StringLength(20, MinimumLength = 1, ErrorMessage = "Waarde moet tussen 1 en 20 liggen.")]
            public string? SearchTerm { get; set; }

            [EnumDataType(typeof(VirtualMachineMode), ErrorMessage = "Ongeldige waarde")]
            public VirtualMachineMode? Status { get; set; }

            [EnumDataType(typeof(OperatingSystemEnum), ErrorMessage ="Ongeldige waarde")]
            public OperatingSystemEnum? OperatingSystem { get; set; }

            [Range(1,1000, ErrorMessage = "Waarde moet tussen 1GB en 1000GB liggen.")]
            public int? MinStorage { get; set; }

            [Range(1, 1000, ErrorMessage = "Waarde moet tussen 1GB en 1000GB liggen.")]
            public int? MaxStorage { get; set; }

            [Range(1, 128, ErrorMessage = "Waarde moet tussen 1GB en 128GB liggen.")]
            public int? MinMemory { get; set; }

            [Range(1, 128, ErrorMessage = "Waarde moet tussen 1GB en 128GB liggen.")]
            public int? MaxMemory { get; set; }

            [Range(1, 50, ErrorMessage = "Waarde moet tussen 1 en 50 liggen.")]
            public int? MinAmountCPU { get; set; }

            [Range(1, 50, ErrorMessage = "Waarde moet tussen 1 en 50 liggen.")]
            public int? MaxAmountCPU { get; set; }

        }

        public class GetDetail
        {
            public int Id { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
        }

        public class Create
        {
            public VirtualMachineDto.Mutate VirtualMachine { get; set; }

            [Required(ErrorMessage ="Waarde mag niet leeg zijn")]
            public int CustomerId { get; set; }
        }

        public class Edit
        {
            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            public int VirtualMachineId { get; set; }

            public VirtualMachineDto.Edit VirtualMachine { get; set; }

        }
    }
}