using Auth0.ManagementApi.Models;
using Domain;
using Domain.Exceptions;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] AuthenticationRequest.Login request)
        {
            var response = await authenticationService.Login(request);

            if (response == null)
            {
                return StatusCode(403);
            }

            return Ok(response);

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] AuthenticationRequest.Register request)
        {
            var response = await authenticationService.Register(request);

            if (response == null)
            {
                return StatusCode(403);
            }

            return Ok(response);

        }

    } 
}
