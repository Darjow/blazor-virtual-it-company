using Bogus;
using Domain.Common;
using Domain.Server;
using Fakers.Server;
using Shared.Servers;
using Shared.VirtualMachines;
using System.Linq;

namespace Services.Server
{
    public class FakeServerService : IFysiekeServerService
    {


        private List<FysiekeServer> _servers;

        public FakeServerService()
        {
            _servers = FysiekeServerFaker.Instance.Generate(3);
        }


        public async Task<FysiekeServerResponse.Details> GetDetailsAsync(FysiekeServerRequest.Detail request)
        {
            FysiekeServerResponse.Details response = new();
            response.Server = new FysiekeServerDto.Detail();


            if (_servers.Any(e => e.Id == request.Id))
            {
                List<VirtualMachineDto.Rapportage> vms = _servers.Find(e => e.Id == request.Id).VirtualMachines.FindAll(e => e.Connection is not null).Select(e => new VirtualMachineDto.Rapportage() { Id = e.Id, Name = e.Name, Statistics = e.Statistics }).ToList();
                response.Server.Id = request.Id;
                response.Server.VirtualMachines = vms;


            }
            else
            {
                response.Server.Id = -1;
            }
            return response;
        }

        public async Task<FysiekeServerResponse.Available> GetAllServers()
        {
            FysiekeServerResponse.Available respons = new();
            respons.Servers = _servers.Select(s => new FysiekeServerDto.Index
            {
                Id = s.Id,
                Name = s.Naam,
                Hardware = s.Hardware,
                HardWareAvailable = s.HardwareAvailable,
                ServerAddress = s.ServerAddress
            }).ToList();

            respons.Count = _servers.Count();
            return respons;
        }

        public async Task<FysiekeServerResponse.ResourcesAvailable> GetAvailableHardWareOnDate(FysiekeServerRequest.Date date)
        {

            FysiekeServerResponse.ResourcesAvailable response = new();
            response.Servers = new List<FysiekeServerDto.Beschikbaarheid>();



            foreach (var server in _servers)
            {
                Hardware max = server.Hardware;
                foreach (var vm in server.VirtualMachines)
                {
                    if (vm.Contract.EndDate < date.FromDate || vm.Contract.StartDate > date.ToDate)
                    {
                        continue;
                    }
                    else
                    {
                        max = new Hardware(max.Memory - vm.Hardware.Memory, max.Storage - vm.Hardware.Storage, max.Amount_vCPU - vm.Hardware.Amount_vCPU);

                    }

                }

                response.Servers.Add(new FysiekeServerDto.Beschikbaarheid() { Id = server.Id, AvailableHardware = max });

            };
            return response;
        }

        //retourneert data voor de volgende 3 maanden
        public async Task<FysiekeServerResponse.GraphValues> GetGraphValueForServer()
        {
            Dictionary<DateTime, Hardware> max = new();

            Hardware maxHardware = GetMaxCapacity();
            DateTime today = DateTime.Now;
            DateTime end = DateTime.Parse($"{today.AddDays(90).Day}/{today.AddDays(90).Month}/{today.AddDays(90).Year} 23:00");
            DateTime start;

            if (_servers.Count > 0)
            {
                foreach (var _server in _servers)
                {
                    if (_server.VirtualMachines.Count > 0)
                    {
                        foreach (var _vm in _server.VirtualMachines)
                        {
                            if (_vm.Contract.EndDate > today)
                            {

                                if (_vm.Contract.StartDate <= today)
                                {
                                    start = DateTime.Parse($"{today.Day}/{today.Month}/{today.Year} 00:00");
                                }
                                else
                                {
                                    start = DateTime.Parse($"{_vm.Contract.StartDate.Day}/{_vm.Contract.StartDate.Month}/{_vm.Contract.StartDate.Year} 00:00");
                                }

                                DateTime value = start;
                                for (int i = 0; i < end.Subtract(start).TotalDays; i++)
                                {
                                    if (!max.ContainsKey(value))
                                    {
                                        max.Add(value, new Hardware(maxHardware.Memory - _vm.Hardware.Memory, maxHardware.Storage - _vm.Hardware.Storage, maxHardware.Amount_vCPU - _vm.Hardware.Amount_vCPU));
                                    }
                                    else
                                    {
                                        Hardware current = max[value];
                                        max.Remove(value);
                                        max.Add(value, new Hardware(current.Memory - _vm.Hardware.Memory, current.Storage - _vm.Hardware.Storage, current.Amount_vCPU - _vm.Hardware.Amount_vCPU));

                                    }
                                    value = value.AddDays(1);
                                }
                            }
                        }
                    }
                }
            }

            return new FysiekeServerResponse.GraphValues() { GraphData = max };
        }



        private Hardware GetMaxCapacity()
        {
            Hardware maxHardware = new Hardware(0, 0, 0);

            foreach (var server in _servers)
            {
                maxHardware.Memory += server.Hardware.Memory;
                maxHardware.Storage += server.Hardware.Storage;
                maxHardware.Amount_vCPU += server.Hardware.Amount_vCPU;
            }
            return maxHardware;

        }
    }
}