using Domain.Common;
using Microsoft.AspNetCore.Components;
using Shared.Users;

namespace Client.Users.Customers;

public partial class Details
{
    private UserDto.EditCustomer model = new();
    public bool Loading = false;
    public bool Edit = false;
    public bool Intern = false;
    private UserDto.KlantDetail Klant;
    [Parameter] public int Id { get; set; }
    [Inject] public IUserService UserService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetKlantAsync();
        ObjectToMutate();
    }

    private async Task GetKlantAsync()
    {
        Loading = true;
        var request = new UserRequest.DetailKlant { Id = Id };
        Klant = await UserService.GetDetailKlant(request);

        if (Klant.Opleiding is not null)
        {
            Intern = true;
        }
        Loading = false;
    }
    public void Toggle()
    {
        Edit = !Edit;
    }


    private async void EditKlant()
    {

        UserRequest.Edit request = new()
        {
            Contactpersoon = model.Contactpersoon,
            Bedrijf = model.Bedrijf,
            Opleiding = model.Opleiding,
            Email = model.Email,
            FirstName = model.FirstName,
            Name = model.Name,
            PhoneNumber = model.PhoneNumber,
            Type = model.Type,

        };
        await UserService.EditAsync(Klant.Id, request);

        Klant = await UserService.GetDetailKlant(new UserRequest.DetailKlant() { Id = Klant.Id });
        ObjectToMutate();
        Toggle();
    }

    public void ObjectToMutate()
    {
        model.FirstName = Klant.FirstName;
        model.Name = Klant.Name;
        model.Email = Klant.Email;
        model.PhoneNumber = Klant.PhoneNumber;
        model.Type = Klant.Type == null ? null : Klant.Type; ;
        model.Opleiding = Klant.Opleiding == null ? null : Klant.Opleiding;
        model.Bedrijf = Klant.Bedrijfsnaam;
        model.Contactpersoon = Klant.ContactPersoon == null ? new ContactDetails() : Klant.ContactPersoon;
        model.ReserveContactpersoon = Klant.ReserveContactPersoon == null ? new ContactDetails() : Klant.ReserveContactPersoon;
    }

    private ContactDetails? ValidateContactDetails(ContactDetails det)
    {
        if(det.GetType().GetProperties().Any(e => e.GetValue(det) == null)){
            return null;
        }
        return det;
    }
}