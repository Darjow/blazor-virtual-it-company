using Auth0.ManagementApi.Models;
using Azure;
using Domain;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Users;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("email")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse.JWTClaims>> GetUserByEmail([FromQuery] string email)
        {
            var user = await userService.GetLoginCredentials(email);

            if (user == null)
            {
                return StatusCode(400);
            }

            return Ok(user);
        }

        [HttpGet("customers")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserResponse.AllKlantenIndex>> GetAllCustomers()
        {
            var users = await userService.GetAllKlanten();

            if(users == null)
            {
                return StatusCode(400);
            }
            return Ok(users);
        }


        [HttpGet("customers/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserResponse.DetailKlant>> GetCustomerById(int Id)
        {
            var user = await userService.GetDetailKlant(new UserRequest.DetailKlant{ Id = Id});

            if (user == null)
            {
                return StatusCode(400);
            }

            var isAdminClaim = User.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Role && e.Value == "Admin");
            var isAdmin = isAdminClaim != null;


            var his = user.Id == Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!isAdmin && !his)
            {
                return Ok(new UserResponse.DetailKlant { Id = -1 });
            }

            return Ok(user);
        }

        [HttpPut("customers/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<UserResponse.Edit>> UpdateCustomer([FromRoute] int id, [FromBody] UserRequest.Edit request)
        {
            var user = await userService.EditAsync(id, request);

            if (user == null)
            {
                return StatusCode(400);
            }
            return Ok(user);
        }


        [HttpGet("admins")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserResponse.AllAdminsIndex>> GetAllAdmins()
        {
            var users = await userService.GetAllAdmins();

            if (users == null)
            {
                return StatusCode(400);
            }
            return Ok(users);
        }

    }
}