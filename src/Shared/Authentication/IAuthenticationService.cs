namespace Shared.Authentication
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResponse> Login(AuthenticationRequest.Login request);
        public Task<AuthenticationResponse> Register(AuthenticationRequest.Register request);

    }
}
