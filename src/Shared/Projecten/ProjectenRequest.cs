using Domain.Users;
using Domain.VirtualMachines;
using Shared.VirtualMachines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Projects
{
    public static class ProjectRequest
    {
        public class All
        { 
            public string? SearchTerm { get; set; } //filter will search on: project name + name of customer

        }

        public class Detail
        {
            public int Id { get; set; }
        }

        public class Delete
        {
            [Required(ErrorMessage = "ProjectId moet meegegeven worden")]
            public int ProjectId { get; set; }
        }

        public class Create: ProjectDto.Mutate
        {
        }

        public class Edit
        {
            [Required(ErrorMessage = "ProjectId moet meegegeven worden")]
            public int ProjectId { get; set; }

            public ProjectDto.Mutate Project { get; set; }

        }
    }
}
