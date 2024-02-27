using Domain.Common;
using System;

namespace Domain
{
    public class Gebruiker : Entity
    {

        public String Name { get; set; }
        public String FirstName { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; } 

        public Gebruiker(string name, string firstname, string phoneNumber, string email, string passwordHash)
        {
            this.Name = name;
            this.FirstName = firstname;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.PasswordHash = passwordHash;
        }
        public Gebruiker() { }

    }
}