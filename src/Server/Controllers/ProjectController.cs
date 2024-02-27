using Domain.Projecten;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Projects;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectenService;

        public ProjectController(IProjectService projectenService)
        {
            this.projectenService = projectenService;
        }


        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ProjectResponse.All>> GetAll([FromQuery] ProjectRequest.All? request)
        {
            var projects = await projectenService.GetIndexAsync(request);

            var isAdminClaim = User.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Role && e.Value == "Admin");
            var isAdmin = isAdminClaim != null;

            if (!isAdmin)
            {
                var id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var temp = projects.Projects.FindAll(e => e.User.Id == id);
                projects.Projects = temp;
                projects.Total = temp.Count;
         
            }
            return Ok(projects);

        }

        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ProjectResponse.Detail>> GetById([FromRoute] ProjectRequest.Detail request)
        {
            var project = await projectenService.GetDetailAsync(request);
            var isAdminClaim = User.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Role && e.Value == "Admin");
            var isAdmin = isAdminClaim != null;

            if (project.Id < 1)
            {
                return StatusCode(400);
            }

            if (isAdmin)
            {
                return Ok(project);
            }

            var id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(project.User.Id != id)
            {
                return StatusCode(403);
            }


            return Ok(project);

        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ProjectResponse.Detail>> Create([FromBody] ProjectRequest.Create request)
        {
            var found = Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id);

            if (!found) //not logged in
            {
                return StatusCode(403);
            }

            if(id != request.CustomerId)
            {
                var isAdminClaim = User.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Role && e.Value == "Admin");
                var isAdmin = isAdminClaim != null;

                if (!isAdmin)
                {
                    return StatusCode(403);
                }
            }

            
            var project = await projectenService.CreateAsync(request);

            if (project.ProjectId < 1)
            {
                return StatusCode(400);
            }

            return Ok(project);

        }
    }
}