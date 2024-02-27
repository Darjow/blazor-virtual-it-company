using Domain.VirtualMachines.VirtualMachine;
using Microsoft.AspNetCore.Components;
using Shared.Users;
using Shared.VirtualMachines;
using Domain.Users;
using Client.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Users.Customers
{
    public partial class CreateKlant
    {
        public UserDto.Create model = new();
        [Inject] public IUserService userService { get; set; }
        [Inject] NavigationManager NavMan { get; set; }

        [Inject] AuthenticationProvider provider { get; set; }
        public Boolean isIntern { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            model.Contactpersoon = new Domain.Common.ContactDetails();
            model.ReserveContactpersoon = new Domain.Common.ContactDetails();
        }

        public void toggleRelation()
        {
            isIntern = !isIntern;
        }

        private async void RegistreerKlant()
        {
            UserRequest.Create request = new()
            {
               Type = model.Type,
               ReserveContactpersoon = model.ReserveContactpersoon,
               PhoneNumber = model.PhoneNumber,
               Email = model.Email,
               Name= model.Name,
               FirstName=model.FirstName,
               Opleiding = model.Opleiding,
               Bedrijf = model.Bedrijf,
               Contactpersoon= model.Contactpersoon,
               Password = model.Password
          
            };
            var created = await userService.CreateAsync(request);


            if (created.Id > 0)
            {
                provider.ChangeAuthenticationState(AuthenticationProvider.Customer(model.Name, model.Email, created.Id));
                NavMan.NavigateTo("/");
            }
        }
    }
}