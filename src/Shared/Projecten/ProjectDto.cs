using Domain.Users;
using Domain.VirtualMachines.VirtualMachine;
using FluentValidation;
using Shared.Users;
using Shared.VirtualMachines;
using System.ComponentModel.DataAnnotations;

namespace Shared.Projects;

public static class ProjectDto
{
    public class Index
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public UserDto.Index User { get; set; }

    }
    public class Detail : Index
    {
        public List<VirtualMachineDto.Index> VirtualMachines { get; set; }

    }

    public class Mutate
    {
        [Required(ErrorMessage = "Naam moet meegegeven worden")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Naam is te klein/te groot")]
        public String Name { get; set; }

        [Required(ErrorMessage = "CustomerId moet meegegeven worden")]
        public int CustomerId { get; set; }


    }
}