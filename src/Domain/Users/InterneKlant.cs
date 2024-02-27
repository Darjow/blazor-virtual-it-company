using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class InterneKlant : Klant
    {

        private Course _opleiding;

        public Course Opleiding
        {
            get { return _opleiding; }
            set { _opleiding = Guard.Against.Null(value, nameof(_opleiding)); }
        }

        public InterneKlant(string name, string firstname, string phoneNumber, string email, string passwordHash, Course opleiding) : base(name, firstname, phoneNumber, email, passwordHash)
        {
            this.Opleiding = opleiding;
        }

        public InterneKlant(): base("", "", "", "", "") { }

    }
}