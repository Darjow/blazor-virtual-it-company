using Ardalis.GuardClauses;
using Domain.Common;
using System;

namespace Domain.VirtualMachines.Contract
{
    public class VMContract : Entity
    {
        private int _customerId;
        private int _vmId;
        private DateTime _startDate;
        private DateTime _endDate;


        public int CustomerId { get { return _customerId; } set { _customerId = Guard.Against.NegativeOrZero(value, nameof(_customerId)); } }
        public int VMId { get { return _vmId; } set { _vmId = Guard.Against.NegativeOrZero(value, nameof(_vmId)); } }
        public DateTime StartDate { get { return _startDate; } set { _startDate = Guard.Against.Null(value, nameof(_startDate)); } }
        public DateTime EndDate { get { return _endDate; } set { _endDate = Guard.Against.Null(value, nameof(_endDate)); } }


        public VMContract(int c_id, int vm_id, DateTime start_d, DateTime end_d)
        {
            CustomerId = c_id;
            VMId = vm_id;
            StartDate = start_d;
            EndDate = end_d;
        }

        public VMContract() { }
    }
}