using Microsoft.AspNetCore.Components;
using Client.Shared;
using global::Shared.Authentication;
using Blazored.LocalStorage;

namespace Client.Authentication
{
    public partial class Login
    {

        [Inject] public IAuthenticationService AuthService { get; set; }

        [Inject] public NavigationManager Navigation { get; set; }

        [Inject] public ILocalStorageService LocalStorage { get; set; }


        private string Error { get; set; } = "";
        private AuthenticationRequest.Login loginDto = new(){Email = String.Empty, Password = String.Empty};

        private async void HandleLogin()
        {
            try
            {
                var loginResponse = await AuthService.Login(loginDto);
                await LocalStorage.SetItemAsStringAsync("jwt_token", loginResponse.Token);
                Navigation.NavigateTo("/", true);
            }
            catch(ApplicationException ex)
            {
                Error = ex.Message;

            } 
        }
    }
}