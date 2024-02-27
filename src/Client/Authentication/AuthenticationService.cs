using Client.Infrastructure;
using Shared.Authentication;
using System.Net.Http.Json;

namespace Client.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly PublicClient publicClient;
        private const string endpoint = "api/authentication";

        public AuthenticationService( PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<AuthenticationResponse> Login(AuthenticationRequest.Login request)
        {
            var response = await publicClient.Client.PostAsJsonAsync($"{endpoint}/login", request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        }

        public async Task<AuthenticationResponse> Register(AuthenticationRequest.Register request)
        {
            var response = await publicClient.Client.PostAsJsonAsync($"{endpoint}/register", request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        }

    }
}
