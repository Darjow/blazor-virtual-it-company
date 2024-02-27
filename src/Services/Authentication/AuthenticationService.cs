using Domain.Users;
using Shared.Authentication;
using Shared.Users;
using Domain.Exceptions;
using Domain.Utility;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly DotNetDbContext _dbContext;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserService userService, DotNetDbContext dbContext, ILogger<AuthenticationService> logger, IConfiguration configuration)
        {
            this._userService = userService;
            this._dbContext = dbContext;
            this._logger = logger;
            this._configuration = configuration;
        }
        public async Task<AuthenticationResponse> Login(AuthenticationRequest.Login request)
        {
            _logger.LogInformation($"Login request received with email: {request.Email}");

            var user = await _userService.GetLoginCredentials(request.Email);

            if (user == null)
            {
                _logger.LogWarning("Login request failed: wrong credentials");
                throw new WrongCredentialsException();
            }

            if (!PasswordUtilities.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login request failed: wrong credentials");
                throw new WrongCredentialsException();

            }
            _logger.LogInformation("Login request was successful.");
            return await Task.FromResult(new AuthenticationResponse { Token = GenerateJwtToken(user) });
        }
     
        public async Task<AuthenticationResponse> Register(AuthenticationRequest.Register request)
        {
            _logger.LogInformation($"Register request received");

            if (request.Password != request.PasswordVerification)
            {
                _logger.LogWarning("Register request failed: passwords did not match");
                throw new WrongCredentialsException("Wachtwoorden komen niet overeen.");
            }

            var user = await _dbContext.Gebruikers.FirstOrDefaultAsync(e => e.Email == request.Email);

            
            if (user != null){
                _logger.LogWarning("Register request failed: user already exists with that email");
                throw new EntityAlreadyExistsException("Gebruiker", "email", request.Email);
            }


            var klant = new InterneKlant(
                request.Lastname, request.Firstname, request.PhoneNumber, request.Email, PasswordUtilities.HashPassword(request.Password), Course.NONE);

            var createdUser = await _dbContext.Klanten.AddAsync(klant);
            await _dbContext.SaveChangesAsync();


          var jwtClaims = new UserResponse.JWTClaims { Email = request.Email, FirstName = request.Firstname, Id = createdUser.Entity.Id  };
            _logger.LogInformation("Register request was successful.");

            return await Task.FromResult(new AuthenticationResponse { Token = GenerateJwtToken(jwtClaims) });
            
        }

        private string GenerateJwtToken(UserResponse.JWTClaims user)
        {
            string secretKey = _configuration["JWTSettings:secretkey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claimEmail = new Claim(ClaimTypes.Email, user.Email);
            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            var claimRole = new Claim(ClaimTypes.Role, user.Role is null ? "Customer" : "Admin");
            var claimName = new Claim(ClaimTypes.Name, user.FirstName);

            var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimNameIdentifier, claimRole, claimName }, "serverAuth");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }



    }
}