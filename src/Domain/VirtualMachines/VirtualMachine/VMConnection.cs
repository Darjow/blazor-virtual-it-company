using Ardalis.GuardClauses;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.VirtualMachines.VirtualMachine
{
    public class VMConnection : Entity
    {

        private string _fqdn;
        private IPAddress _hostname;
        private string _username;
        private string _passwordHash;
        private string _password;

        public string FQDN { get { return _fqdn; } set { _fqdn = Guard.Against.NullOrEmpty(value, nameof(_fqdn)); } }
        public IPAddress Hostname { get { return _hostname; } set { _hostname = Guard.Against.Null(value, nameof(_hostname)); } }
        public string Username { get { return _username; } set { _username = Guard.Against.NullOrEmpty(value, nameof(_username)); } }
        public String PasswordHash { get { return _passwordHash; } set { _passwordHash = Guard.Against.NullOrEmpty(value, nameof(_passwordHash)); } }

        //NOT SAVED IN DB, JUST FOR SHOWING TO CUSTOMER AS UNHASHING IS NOT POSSIBLE
        public String Password { get { return _password; } set { _password = Guard.Against.NullOrEmpty(value, nameof(_password)); } }

        public VMConnection(string FQDN, IPAddress hostname, string username, string password, string passwordHash)
        {
            this.FQDN = FQDN;
            Hostname = hostname;
            Username = username;
            Password = password;
            PasswordHash = passwordHash;
        }

        public VMConnection() { }


    }
}