using Microsoft.AspNetCore.Components;
using Shared.Servers;

namespace Client.Servers;

public partial class Index
{
    [Inject] public IFysiekeServerService FysiekeServerService { get; set; }
    [Inject] public NavigationManager Router { get; set; }
    private List<FysiekeServerDto.Index> Servers { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await FysiekeServerService.GetAllServers();
        Servers = response.Servers;
    }

    public void RedirectToDetailsPage(int id)
    {
        Router.NavigateTo($"servers/{id}");

    }
}