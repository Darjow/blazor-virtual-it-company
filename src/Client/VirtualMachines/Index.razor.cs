using Append.Blazor.Sidepanel;
using Microsoft.AspNetCore.Components;
using Shared.Projects;


namespace Client.VirtualMachines
{
    public partial class Index
    {
        [Inject] public IProjectService ProjectService { get; set; }
        [Inject] public ISidepanelService Sidepanel { get; set; }


        [Inject] NavigationManager Router { get; set; }

        private List<ProjectDto.Index> _projects;

        private Dictionary<int, ProjectDto.Detail> _details = new Dictionary<int, ProjectDto.Detail>();

        protected override async Task OnInitializedAsync()
        {

            ProjectRequest.All request = new();

            var response = await ProjectService.GetIndexAsync(request);
            _projects = response.Projects;
        }


        public async Task GetVirtualMachines(int id)
        {
            ProjectRequest.Detail request = new() { Id = id };

            var response = await ProjectService.GetDetailAsync(request);
            ProjectDto.Detail resp = new ProjectDto.Detail()
            {
                Id = response.Id,
                User = response.User,
                Name = response.Name,
                VirtualMachines = response.VirtualMachines
            };



            _details.Add(id, resp);


        }
        public void NavigateToVMDetails(int id)
        {
            Router.NavigateTo("virtualmachine/" + id);
        }

    }
}