using Domain.Common;
using Domain.Users;
using Domain.Utility;
using Domain.Utility;
using FluentValidation;
using Shared.Authentication;
using Shared.Projects;
using System.ComponentModel.DataAnnotations;

namespace Shared.Users;

public static class UserDto
{

    public class Index
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class KlantDetail : Index
    {
        public string? Bedrijfsnaam { get; set; }
        public Course? Opleiding { get; set; }
        public List<ProjectDto.Index>? Projects { get; set; }

        public  BedrijfType? Type { get; set; }
        public ContactDetails? ContactPersoon { get; set; }
        public ContactDetails? ReserveContactPersoon { get; set; }
    }

    public class EditCustomer
    {
        [Required(ErrorMessage = "Je moet een voornaam ingeven")]
        [StringLength(20, ErrorMessage = "Naam is te lang")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Je moet een naam ingeven.")]
        public string Name { get; set; }

        [RegularExpression(@"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$", 
        ErrorMessage = "Dit is geen geldig telefoonnummer")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Je moet een email ingeven")]
        [EmailAddress(ErrorMessage = "Geen geldige email")]
        public string Email { get; set; }

        [EnumDataType(typeof(Course), ErrorMessage = "Geen geldige waarde")]
        public Course? Opleiding { get; set; }

        public string? Bedrijf { get; set; }

        [EnumDataType(typeof(BedrijfType), ErrorMessage = "Geen geldige waarde")]
        public BedrijfType? Type { get; set; }
        public ContactDetails? Contactpersoon { get; set; }
        public ContactDetails? ReserveContactpersoon { get; set; }
    }
    public class Create
    {
        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [StringLength(50, ErrorMessage = "Waarde moet kleiner zijn dan 50.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [StringLength(50, ErrorMessage = "Waarde moet kleiner zijn dan 50.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
        ErrorMessage = "Wachtwoord moet minstens 8 characters lang zijn en zowel een hoofdletter, kleine letter en cijfer bevatten")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [EmailAddress(ErrorMessage = "Geen geldige email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [RegularExpression(@"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$",
        ErrorMessage = "Dit is geen geldig telefoonnummer")]
        public string PhoneNumber { get; set; }
        [EnumDataType(typeof(Course), ErrorMessage = "Geen geldige waarde")]
        public Course? Opleiding { get; set; }

        public string? Bedrijf { get; set; }

        [EnumDataType(typeof(BedrijfType), ErrorMessage = "Geen geldige waarde")]
        public BedrijfType? Type { get; set; }
        public ContactDetails? Contactpersoon { get; set; }
        public ContactDetails? ReserveContactpersoon { get; set; }
    }
}