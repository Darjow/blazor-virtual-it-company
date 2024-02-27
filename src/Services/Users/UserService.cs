using Azure.Core;
using Domain;
using Domain.Exceptions;
using Domain.Users;
using Domain.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Shared.Projects;
using Shared.Users;

namespace Services.Users
{
    public class UserService : IUserService
    {
        private readonly DbSet<Gebruiker> _gebruikers;
        private readonly DbSet<Klant> _klanten;
        private readonly DbSet<Administrator> _admins;
        private readonly ILogger<UserService> _logger;
        

        private readonly DotNetDbContext _dbContext;

        public UserService(DotNetDbContext dbContext, ILogger<UserService> logger)
        {
            this._dbContext = dbContext;
            this._klanten = dbContext.Klanten;
            this._admins = dbContext.Admins;
            this._gebruikers = dbContext.Gebruikers;
            this._logger = logger;
        }


        public async Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
        {
            _logger.LogInformation($"Request received: CreateAsync(...)");
            Klant k;

            if (request.Opleiding.HasValue)
            {
                k = new InterneKlant(
                    request.Name,
                    request.FirstName,
                    request.PhoneNumber,
                    request.Email,
                    PasswordUtilities.HashPassword(request.Password),
                    request.Opleiding.Value);
                _klanten.Add(k);
            }
            else
            {
                k = new ExterneKlant(
                   request.Name,
                   request.FirstName,
                   request.PhoneNumber,
                   request.Email,
                   PasswordUtilities.HashPassword(request.Password),
                   request.Bedrijf,
                   request.Type.Value,
                   request.Contactpersoon,
                   request.ReserveContactpersoon);
                _klanten.Add(k);
            }
            _dbContext.SaveChanges();

            _logger.LogInformation("Request was successful");


            return new UserResponse.Create
            {

                Id = k.Id,
                Email = k.Email,
                FirstName = k.FirstName,
                Name = k.Name,
                PhoneNumber = k.PhoneNumber
            };
                
            
        }

        public async Task<UserResponse.Edit> EditAsync(int id, UserRequest.Edit request)
        {
            _logger.LogInformation($"Request received: EditAsync({id}, ...)");

            Klant klant = await _klanten.SingleOrDefaultAsync(e => e.Id == id);

            if(klant == null)
            {
                _logger.LogWarning($"Request failed: No user found with id: {id}.");
                throw new EntityNotFoundException("Klant", "id", id);
            }

            if(klant is ExterneKlant externe)
            {
                externe.Contactpersoon = request.Contactpersoon;
                externe.ReserveContactpersoon = request.ReserveContactpersoon;
            }
            
            klant.FirstName = request.FirstName;
            klant.Name = request.Name;
            klant.Email = request.Email;
            klant.PhoneNumber = request.PhoneNumber;
            _dbContext.SaveChanges();

            _logger.LogInformation("Request was successful");

            return new UserResponse.Edit { Id = klant.Id };



        }

        public async Task<UserResponse.AllKlantenIndex> GetAllKlanten()
        {
            _logger.LogInformation($"Request received: GetAllKlanten()");

            UserResponse.AllKlantenIndex response = new();
            response.Klanten = await _klanten.Select(x => new UserDto.Index
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();
            response.Total = _klanten.Count();

            _logger.LogInformation("Request was successful");

            return response;
        }

        public async Task<UserResponse.DetailKlant> GetDetailKlant(UserRequest.DetailKlant request)
        {
            _logger.LogInformation($"Request received: GetDetailsKlant({request.Id})");

            var k = await _klanten.FirstOrDefaultAsync(x => x.Id == request.Id);

       
            if (k is null)
            {
                _logger.LogWarning($"Request failed: No customer found with id: {request.Id}.");
                throw new EntityNotFoundException("Klant", "id", request.Id);

            }

            List<ProjectDto.Index> projecten = new();
            UserResponse.DetailKlant response = new()
            {
                Id = k.Id,
                Name = k.Name,
                FirstName = k.FirstName,
                Email = k.Email,
                PhoneNumber = k.PhoneNumber,
                Projects = projecten,
            };


            k.Projecten.ForEach(p => projecten.Add(new ProjectDto.Index()
            {
                Id = p.Id,
                Name = p.Name,
                User = new UserDto.Index
                {
                    Email = k.Email,
                    FirstName = k.FirstName,
                    Id = k.Id,
                    Name = k.Name,
                    PhoneNumber = k.PhoneNumber
                }
            }));


            if (k is InterneKlant)
            {
                response.Opleiding = ((InterneKlant)k).Opleiding;
            }
            else
            {
                ExterneKlant kE = await _dbContext.ExterneKlanten.Include(e => e.Contactpersoon).FirstOrDefaultAsync(e => e.Id == request.Id); //extra request to include contactpersoon table

                response.Bedrijfsnaam = kE.Bedrijfsnaam;
                response.Type = kE.Type;
                response.ContactPersoon = kE.Contactpersoon;
                response.ReserveContactPersoon = kE.ReserveContactpersoon;

            }

            _logger.LogInformation("Request was successful");

            return response;
        }

        public async Task<UserResponse.JWTClaims> GetLoginCredentials(string email)
        {
            _logger.LogInformation($"Request received: GetLoginCredentials({email})");
            var user = _gebruikers.FirstOrDefault(x => x.Email == email);
      
            if (user == null)
            {
                _logger.LogWarning($"Request failed: No user found with email: {email}.");
                throw new WrongCredentialsException();
            }

            var response = new UserResponse.JWTClaims
            {
                PasswordHash = user.PasswordHash,
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email
            };

            if (user is Administrator)
            {
                Administrator _user = (Administrator)user;
                response.Role = _user.Role;

            }
            _logger.LogInformation("Request was successful");

            return await Task.FromResult(response);


        }

        public async Task<UserResponse.AllAdminsIndex> GetAllAdmins()
        {
            _logger.LogInformation($"Request received: GetAllAdmins()");
            UserResponse.AllAdminsIndex response = new();
            response.Admins = await _admins.Select(x => new AdminUserDto.Index {
                FirstName = x.FirstName,
                Id = x.Id,
                Name = x.Name,
                Role = x.Role,
                Email = x.Email
            }).ToListAsync();

            response.Total = response.Admins.Count();
            _logger.LogInformation("Request was successful");

            return response;

        }

 
    }
}