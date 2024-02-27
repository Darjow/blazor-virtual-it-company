using Domain.Common;
using Shared.VirtualMachines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Servers
{
    public static class FysiekeServerRequest
    {
        public class Detail
        {
            [Required(ErrorMessage = "Id moet meegegeven worden")]
            public int Id { get; set; }
        }
        public class Date
        {
            [Required(ErrorMessage = "Startdatum moet meegegeven worden")]
            public DateTime FromDate { get; set; }

            [Required(ErrorMessage = "Startdatum moet meegegeven worden")]
            public DateTime ToDate { get; set; }
        }

    }
}