using Ardalis.GuardClauses;
using Domain.Common;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Users
{
    public class ExterneKlant : Klant
    {

        private string _bedrijfsNaam;
        private BedrijfType _bedrijfType;
        private ContactDetails _c1;
        private ContactDetails _c2;
        public String Bedrijfsnaam { get { return _bedrijfsNaam; } set { _bedrijfsNaam = Guard.Against.NullOrEmpty(value, nameof(_bedrijfsNaam)); } }
        public BedrijfType Type { get { return _bedrijfType; } set { _bedrijfType = Guard.Against.Null(value, nameof(_bedrijfType)); } }

        public ContactDetails? Contactpersoon { get; set; }
        public ContactDetails? ReserveContactpersoon { get; set; }



        public ExterneKlant(string name, string firstname, string phoneNumber, string email, string passwordHash, string bedrijfsnaam, BedrijfType bt, ContactDetails? c1, ContactDetails? c2) : base(name, firstname, phoneNumber, email, passwordHash)
        {
            this.Bedrijfsnaam = bedrijfsnaam;
            this.Type = bt;
            this.Contactpersoon = c1;
            this.ReserveContactpersoon = c2;
        }

        public ExterneKlant() : base("", "", "", "", "") {}



    }
}