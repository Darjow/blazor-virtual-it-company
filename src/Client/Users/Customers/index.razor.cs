using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shared.Users;

namespace Client.Users.Customers;

public partial class Index
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    private List<UserDto.Index> Klanten { get; set; }
    private bool Loading { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        var response = await UserService.GetAllKlanten();
        Klanten = response.Klanten;
        Loading = false;
    }
    private void NavToDetail(int id)
    {
        NavigationManager.NavigateTo($"klant/{id}");
    }
}