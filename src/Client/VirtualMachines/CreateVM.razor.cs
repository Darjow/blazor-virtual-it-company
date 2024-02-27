using Domain.Common;
using Domain.VirtualMachines.BackUp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Projects;
using Shared.Users;
using Shared.VirtualMachines;
using System.Security.Claims;

namespace Client.VirtualMachines
{
    public partial class CreateVM
    {
        public VirtualMachineDto.Mutate model = new();
        [Inject] public IUserService userService { get; set; }
        [Inject] public IVirtualMachineService vmService { get; set; }

        [Inject] public IProjectService projService { get; set; } 
        [Inject] NavigationManager NavMan { get; set; }
        [Inject] AuthenticationStateProvider provider { get; set; }

        private ProjectResponse.All projects;
        private bool loading = false;
        private int userid = 0;

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            var user = await provider.GetAuthenticationStateAsync();
            var claim = user.User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            userid = Int32.Parse(claim.ToString());
            

            model.Hardware = new Hardware(0, 0, 0);
            model.Backup = new Backup(BackUpType.GEEN, null);
            model.Start = DateTime.Now;
            model.End = DateTime.Now;

            projects = await projService.GetIndexAsync(new ProjectRequest.All());
            loading = false;
        }



        private async void VMAanvragen()
        {
         
            VirtualMachineRequest.Create request = new()
            {
                VirtualMachine = model,
                CustomerId = userid
            };
            await vmService.CreateAsync(request);



            NavMan.NavigateTo($"/virtualmachines");
        }
    }
}