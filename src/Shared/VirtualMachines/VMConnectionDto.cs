﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.VirtualMachines
{
    public class VMConnectionDto
    {
        public class Detail
        {
            public string FQDN { get; set; }
            public string Hostname { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

        }
    }
}
