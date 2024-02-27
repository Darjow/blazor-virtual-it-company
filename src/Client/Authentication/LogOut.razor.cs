using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

namespace Client.Authentication
{
    public partial class LogOut
    {
        [Inject] public ILocalStorageService localStorageService { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        public bool Loading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            var token = await localStorageService.GetItemAsStringAsync("jwt_token");

            if (!String.IsNullOrEmpty(token))
            {
                await localStorageService.RemoveItemAsync("jwt_token");
                nav.NavigateTo("/logout", true);

            }
            Loading = false;

        }
    }
}