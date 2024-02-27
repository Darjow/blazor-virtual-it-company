using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Users;
using Domain.VirtualMachines.VirtualMachine;
using Domain.Utility;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Domain.Server
{
    public class FysiekeServer : Entity
    {

        private readonly List<VirtualMachine> _vms = new(); //only contains active VMs running on this server

        private string _name;
        private string _serverAddress;
        private TotalHardware _hardWare;


        public String Naam { get { return _name; } private set { _name = Guard.Against.NullOrEmpty(value, nameof(_name)); } }
        public String ServerAddress { get { return _serverAddress; } private set { _serverAddress = Guard.Against.NullOrEmpty(value, nameof(_serverAddress)); } }
        public TotalHardware Hardware { get { return _hardWare; } private set { _hardWare = Guard.Against.Null(value, nameof(_hardWare)); } }
        public AvailableHardware HardwareAvailable { get; set; }
        public List<VirtualMachine> VirtualMachines { get; private set; }


        public FysiekeServer(string naam, TotalHardware hardware, string serverAddress)
        {

            this.Naam = naam;
            this.Hardware = hardware;
            this.ServerAddress = serverAddress;
            this.HardwareAvailable = new AvailableHardware(hardware.Memory, hardware.Storage, hardware.Amount_vCPU);
            this.VirtualMachines = new();
        }

        public FysiekeServer() { }

        public void AddConnection(VirtualMachine vm)
        {

            string password = PasswordUtilities.GeneratePassword(12, 3, 3, 3, 2);

            vm.Connection = new VMConnection(ServerAddress, GetRandomIpAddress(), "admin", password, PasswordUtilities.HashPassword(password));
            if(_vms.FirstOrDefault(e => e.Id == vm.Id) == null)
            {
                _vms.Add(vm);
                HardwareAvailable.Memory -= vm.Hardware.Memory;
                HardwareAvailable.Storage -= vm.Hardware.Storage;
                HardwareAvailable.Amount_vCPU -= vm.Hardware.Amount_vCPU;

            }
        }

        //IP om te connecteren met de VM
        public IPAddress GetRandomIpAddress()
        {
            var random = new Random();
            string ip = $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";


            return IPAddress.Parse(ip);

        }

    }
}