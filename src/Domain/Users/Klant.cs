using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Projecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public abstract class Klant : Gebruiker
    {


        private List<Project> _projecten = new();

        public List<Project> Projecten { get { return _projecten; } }


        public Klant(string name, string firstname, string phoneNumber, string email, string passwordHash) : base(name, firstname, phoneNumber, email, passwordHash)
        {
        }

        public void addProject(Project p)
        {
            if (_projecten == null)
            {
                _projecten = new List<Project>();
            }
            _projecten.Add(p);
        }


    }
}