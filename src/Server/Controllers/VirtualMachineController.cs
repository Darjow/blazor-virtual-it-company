using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.VirtualMachines;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VirtualMachineController : ControllerBase
    {
        private readonly IVirtualMachineService virtualMachineService;

        public VirtualMachineController(IVirtualMachineService virtualMachineService)
        {
            this.virtualMachineService = virtualMachineService;
        }


        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<VirtualMachineResponse.GetDetail>> GetById([FromRoute] VirtualMachineRequest.GetDetail request)
        {
            var response = await virtualMachineService.GetDetailAsync(request);

            if (response.Id < 1)
            {
                return StatusCode(400);
            }

            var isAdminClaim = User.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Role && e.Value == "Admin");
            var isAdmin = isAdminClaim != null;


            var his = response.Contract.CustomerId == Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!isAdmin && !his)
            {
                return StatusCode(403);
            }




            return Ok(response);

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<VirtualMachineResponse.Create>> Create(VirtualMachineRequest.Create request)
        {
            var vm = await virtualMachineService.CreateAsync(request);

            if(vm.Id < 1)
            {
                return StatusCode(400);
            }

            return Ok(vm);
        }


        [HttpGet("graph/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<VirtualMachineResponse.Rapport>> GetGraphs([FromRoute] VirtualMachineRequest.GetDetail request)
        {
            var vmdetails = await virtualMachineService.RapporteringAsync(request);

            if(vmdetails.VirtualMachine.Id < 1)
            {
                return StatusCode(400);
            }

            return Ok(vmdetails);
        }
    }
}
    