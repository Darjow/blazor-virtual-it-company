using Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Servers;
using System;
using System.Threading.Tasks;


namespace Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class FysiekeServerController : ControllerBase
    {

        private readonly IFysiekeServerService fysiekeServerService;

        public FysiekeServerController(IFysiekeServerService fysiekeServerService)
        {
            this.fysiekeServerService = fysiekeServerService;
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<FysiekeServerResponse.Details>> GetById([FromRoute] int Id)
        {
            var server = await fysiekeServerService.GetDetailsAsync(new FysiekeServerRequest.Detail { Id = Id });

            if (server.Server.Id < 1)
            {
                return StatusCode(400);
            }

            return Ok(server);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<FysiekeServerResponse.Available>> GetAll()
        {
            return Ok(await fysiekeServerService.GetAllServers());

        }
        [HttpPost("date")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<FysiekeServerResponse.ResourcesAvailable>> GetAvailableHardWareOnDate([FromBody] FysiekeServerRequest.Date date)
        {
            var hardware = await fysiekeServerService.GetAvailableHardWareOnDate(date); 

            if (hardware.Servers.Count < 1)
            {
                return StatusCode(500); //unexpected errror
            }
            return Ok(hardware);
        }

        [HttpGet("graph")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<FysiekeServerResponse.GraphValues>> GetGraphValueForServer()
        {
            var values = await fysiekeServerService.GetGraphValueForServer();


            if(values.GraphData.Count < 1)
            {
                return StatusCode(400);
            }

            return Ok(values);

        }

    }
 }