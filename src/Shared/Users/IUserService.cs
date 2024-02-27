using Domain;
using static Shared.Users.UserResponse;

namespace Shared.Users {

    public interface IUserService
    {
        Task<UserResponse.AllKlantenIndex> GetAllKlanten();
        Task<UserResponse.AllAdminsIndex> GetAllAdmins();
        Task<UserResponse.DetailKlant> GetDetailKlant(UserRequest.DetailKlant request);
        Task<UserResponse.Edit> EditAsync(int number, UserRequest.Edit request);
        Task<UserResponse.Create> CreateAsync(UserRequest.Create request);
        Task<UserResponse.JWTClaims> GetLoginCredentials(string email);


    }
}
