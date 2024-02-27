using Ardalis.GuardClauses;
using Domain.Users;
using Domain.Utility;
using System;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class ContactDetails : Entity
    {

        private string _phoneNumber;
        private string _email;
        private string _fName;
        private string _lName;

        public String PhoneNumber { get { return _phoneNumber; } set { if (PropertyValidator.IsPhoneNumberValid(value)) _phoneNumber = value; } }
        public String Email { get { return _email; } set { if (PropertyValidator.IsValidEmail(value)) _email = value; } }
        public String FirstName { get { return _fName; } set { _fName = Guard.Against.NullOrEmpty(value, nameof(_fName)); } }
        public String LastName { get { return _lName; } set { _lName = Guard.Against.NullOrEmpty(value, nameof(_lName)); } }


        public ContactDetails() { }

        public ContactDetails(string phone, string email, string fName, string lName)
        {
            this.PhoneNumber = phone;
            this.Email = email;
            this.FirstName = fName;
            this.LastName = lName;

        }
    }
}
