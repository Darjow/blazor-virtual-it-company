using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Projecten;
using Domain.Server;
using Domain.VirtualMachines.BackUp;
using Domain.VirtualMachines.Contract;
using Domain.Statistics;
using System.Net;
using Domain.Utility;
using System;

namespace Domain.VirtualMachines.VirtualMachine
{
    public class VirtualMachine : Entity
    {

        private string _name;
        private OperatingSystemEnum _operatingSystem;
        private VirtualMachineMode _mode;
        private FysiekeServer? _server;
        private Statistic _statistics;


        public string Name { get { return _name; } set { _name = Guard.Against.NullOrEmpty(value, nameof(_name)); } }
        public OperatingSystemEnum OperatingSystem { get { return _operatingSystem; } set { _operatingSystem = Guard.Against.Null(value, nameof(_operatingSystem)); } }
        public VirtualMachineMode Mode { get { return _mode; } set { _mode = Guard.Against.Null(value, nameof(_mode)); } }
        public Hardware Hardware { get; set; }
        public Backup BackUp { get; set; }
        public VMConnection? Connection { get; set; }
        public VMContract? Contract { get; set; }
        public FysiekeServer? FysiekeServer { get { return _server; } set { _server = Guard.Against.Null(value, nameof(_server)); } }

        public Statistic Statistics { get { return _statistics; } set { _statistics = Guard.Against.Null(value, nameof(_statistics)); } }

        public VirtualMachine(string n, OperatingSystemEnum os, Hardware h, Backup b)
        {
            Name = n;
            OperatingSystem = os;
            Hardware = h;
            BackUp = b;
            Mode = VirtualMachineMode.WAITING_APPROVEMENT;
        }

        public VirtualMachine() { }

        public void AddConnection(string FQDN, IPAddress hostname, string username, string password)
        {

 
            string hash = PasswordUtilities.HashPassword(password);

            Connection = new VMConnection(FQDN, hostname, username,password,  hash);
            Mode = VirtualMachineMode.READY;
        }



    }
}