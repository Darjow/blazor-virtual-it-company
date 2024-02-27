using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shared.Users;

namespace Client.Users.Admins;

public partial class Index
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    private List<AdminUserDto.Index> Admins { get; set; }
    private bool Loading { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        var response = await UserService.GetAllAdmins();
        Admins = response.Admins;
        Loading = false;
    }
 
}