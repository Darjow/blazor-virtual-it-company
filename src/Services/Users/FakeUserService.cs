using Domain;
using Domain.Common;
using Domain.Exceptions;
using Domain.Users;
using Domain.Utility;
using Fakers.User;
using Shared.Projects;
using Shared.Users;
using System.Security.Claims;

namespace Services.Users
{
    public class FakeUserService : IUserService
    {
        private List<Klant> _klanten;
        private List<Administrator> _admins;


        public FakeUserService()
        {
            _klanten = UserFaker.Klant.Instance.Generate(20);
            _admins = UserFaker.Administrators.Instance.Generate(3);

        }

        public async Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
        {
            await Task.Delay(100);

            Klant k;

            if (request.Opleiding.HasValue)
            {
                k = new InterneKlant(
                    request.Name,
                    request.FirstName,
                    request.PhoneNumber,
                    request.Email,
                    PasswordUtilities.HashPassword("Klant.1"),
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
                   PasswordUtilities.HashPassword("Klant.1"),
                   request.Bedrijf,
                   request.Type.Value,
                   request.Contactpersoon,
                   request.ReserveContactpersoon);
                _klanten.Add(k);
            }

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
            await Task.Delay(100);
            Klant klant = _klanten.Find(k => k.Id == id);
            klant.FirstName = request.FirstName;
            klant.Name = request.Name;
            klant.Email = request.Email;
            klant.PhoneNumber = request.PhoneNumber;

            return new UserResponse.Edit { Id = klant.Id };
        }

        public async Task<UserResponse.AllKlantenIndex> GetAllKlanten()
        {
            await Task.Delay(100);
            UserResponse.AllKlantenIndex response = new();
            response.Klanten = _klanten.Select(x => new UserDto.Index
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
            }).ToList();
            response.Total = _klanten.Count;

            return response;
        }

        public async Task<UserResponse.DetailKlant> GetDetailKlant(UserRequest.DetailKlant request)
        {
            await Task.Delay(100);
            var k = _klanten.Single(x => x.Id == request.Id);

            if(k is null)
            {
                return new UserResponse.DetailKlant { Id = -1 };
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
                User = new UserDto.Index()
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
                ExterneKlant kE = (ExterneKlant)k;
                response.Bedrijfsnaam = kE.Bedrijfsnaam;
                response.Type = kE.Type;
                response.ContactPersoon = kE.Contactpersoon;
                response.ReserveContactPersoon = kE.ReserveContactpersoon;

            }
        
        return response;
    }

        public Task<UserResponse.JWTClaims> GetLoginCredentials(string email)
        {
            var user = _admins.Cast<Gebruiker>().Concat(_klanten.Cast<Gebruiker>()).FirstOrDefault(u => u.Email == email);
            if(user is not null)
            {
                return Task.FromResult(new UserResponse.JWTClaims { 
                        PasswordHash = user.PasswordHash, 
                });

            }
            return null;

        }

        public Task<UserResponse.AllAdminsIndex> GetAllAdmins()
        {
            throw new NotImplementedException();
        }
    }
}