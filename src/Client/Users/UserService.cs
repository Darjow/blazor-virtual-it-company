using Client.Infrastructure;
using Shared.Users;
using System.Net.Http.Json;

namespace Client.Users
{
    public class UserService : IUserService
    {
        private readonly PublicClient publicClient;
        private readonly HttpClient authenticatedClient;
        private const string endpoint = "api/user";

        public UserService(HttpClient authenticatedClient, PublicClient publicClient)
        {
            this.authenticatedClient = authenticatedClient;
            this.publicClient = publicClient;
        }

        public async Task<UserResponse.AllKlantenIndex> GetAllKlanten()
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/customers");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<UserResponse.AllKlantenIndex>();
        }

        public async Task<UserResponse.DetailKlant> GetDetailKlant(UserRequest.DetailKlant request)
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/customers/{request.Id}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<UserResponse.DetailKlant>();
        }

        public async Task<UserResponse.Edit> EditAsync(int id, UserRequest.Edit request)
        {
            var response = await authenticatedClient.PutAsJsonAsync($"{endpoint}/customers/{id}", request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<UserResponse.Edit>();
        }

        public Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse.JWTClaims> GetLoginCredentials(string email)
        {
            var response = await publicClient.Client.GetAsync($"{endpoint}/email?email={email}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<UserResponse.JWTClaims>();
        }

        public async Task<UserResponse.AllAdminsIndex> GetAllAdmins()
        {
            var response =  await authenticatedClient.GetAsync($"{endpoint}/admins");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<UserResponse.AllAdminsIndex>();
        }
    }
}
