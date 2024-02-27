using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Users;
using Domain.VirtualMachines.VirtualMachine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Projecten
{

    public class Project : Entity
    {

        private List<VirtualMachine> _vms = new(); //contains all VMS on a certain project | inclusive not approved.

        private string _name;
        private Klant _klant;



        public String Name { get { return _name; } set { _name = Guard.Against.NullOrEmpty(value, nameof(_name)); } }
        public Klant Klant { get { return _klant; } set { _klant = Guard.Against.Null(value, nameof(_klant)); } }
        public List<VirtualMachine> VirtualMachines { get { return _vms; } set { _vms = Guard.Against.Null(value, nameof(_vms)); } }

        public Project(string name)
        {
            this.Name = name;
        }
        public VirtualMachine GetVirtualMachineById(int id)
        {
            return _vms.First(e => e.Id == id);
        }


        // name = substring dus meerdere mogelijkheden 
        public List<VirtualMachine> GetVirtualMachineByName(string name)
        {
            return _vms.FindAll(e => e.Name.ToLower().Contains(name));

        }
        public void AddVirtualMachine(VirtualMachine vm)
        {
            _vms.Add(vm);
        }
        public void RemoveVirtualMachine(VirtualMachine vm)
        {
            _vms.Remove(vm);
        }
    }
}