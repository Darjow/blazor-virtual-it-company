using System.ComponentModel.DataAnnotations;

namespace Shared.Authentication
{
    public static class AuthenticationRequest
    {
        public class Login
        {
            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Wachtwoord moet minstens 8 characters lang zijn en zowel een hoofdletter, kleine letter en cijfer bevatten")]
            public string Password { get; set; }

        }
        public class Register
        {
            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [StringLength(50, ErrorMessage = "Waarde moet kleiner zijn dan 50.")]
            public string Firstname { get; set; }

            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [StringLength(50, ErrorMessage = "Waarde moet kleiner zijn dan 50.")]
            public string Lastname { get; set; }

            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Wachtwoord moet minstens 8 characters lang zijn en zowel een hoofdletter, kleine letter en cijfer bevatten")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
            ErrorMessage = "Wachtwoord moet minstens 8 characters lang zijn en zowel een hoofdletter, kleine letter en cijfer bevatten")]
            public string PasswordVerification { get; set; }


            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [EmailAddress(ErrorMessage = "Geen geldige email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
            [RegularExpression(@"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$",
            ErrorMessage = "Dit is geen geldig telefoonnummer")]
            public string PhoneNumber { get; set; }

        }

    }
}