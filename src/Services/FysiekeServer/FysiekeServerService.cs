using System.Linq;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Shared.Servers;
using Shared.VirtualMachines;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Azure.Core;

namespace Services.FysiekeServers
{
    public class FysiekeServerService : IFysiekeServerService
    {
        private readonly DotNetDbContext _dbContext;
        private readonly ILogger<FysiekeServerService> _logger;

        public FysiekeServerService(DotNetDbContext dbContext, ILogger<FysiekeServerService> logger)
        {
            _dbContext = dbContext;
            _logger = logger; 
        }


        public async Task<FysiekeServerResponse.Details> GetDetailsAsync(FysiekeServerRequest.Detail request)
        {
            _logger.LogInformation($"Request received: GetDetailsAsync({request.Id})");

            FysiekeServerResponse.Details response = new();
            response.Server = new FysiekeServerDto.Detail();

            var server = await _dbContext.FysiekeServers.Include(e => e.VirtualMachines).FirstOrDefaultAsync(e => e.Id == request.Id);

            if(server == null)
            {
                _logger.LogWarning($"Request failed: No project found with id: {request.Id}.");
                throw new EntityNotFoundException("Fysieke Server", "id", request.Id);
            }

            List<VirtualMachineDto.Rapportage> vms =
                server.VirtualMachines
                    .Select(e => new VirtualMachineDto.Rapportage()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Statistics = e.Statistics
                    }).ToList();

            response.Server.Id = request.Id;
            response.Server.VirtualMachines = vms;

            _logger.LogInformation("Request was successful");
            return response;
        }

        public async Task<FysiekeServerResponse.Available> GetAllServers()
        {
            _logger.LogInformation("Request received: GetAllServers()");
            FysiekeServerResponse.Available response = new();
            response.Servers = await _dbContext.FysiekeServers.Select(s => new FysiekeServerDto.Index
            {
                Id = s.Id,
                Name = s.Naam,
                Hardware = new TotalHardware(s.Hardware.Memory, s.Hardware.Storage, s.Hardware.Amount_vCPU),
                HardWareAvailable = new AvailableHardware(s.HardwareAvailable.Memory, s.HardwareAvailable.Storage, s.HardwareAvailable.Amount_vCPU),
                ServerAddress = s.ServerAddress
            }).ToListAsync();

            response.Count = response.Servers.Count();
            _logger.LogInformation("Request was successful");
            return response;
        }

        public async Task<FysiekeServerResponse.ResourcesAvailable> GetAvailableHardWareOnDate(FysiekeServerRequest.Date date)
        {
            _logger.LogInformation($"Request received: GetAvailableHardwareOnDate({date.FromDate}, {date.ToDate})");
            FysiekeServerResponse.ResourcesAvailable response = new();
            response.Servers = new List<FysiekeServerDto.Beschikbaarheid>();

            var servers = await _dbContext.FysiekeServers.Include(e => e.VirtualMachines).ThenInclude(e => e.Contract).ToListAsync();


            foreach (var server in servers)
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

            _logger.LogInformation("Request was successful");

            return response;
        }

        public async Task<FysiekeServerResponse.GraphValues> GetGraphValueForServer()
        {
            _logger.LogInformation($"Request received: GetGraphValueForServer()");

            Dictionary<DateTime, Hardware> max = new();

            Hardware maxHardware = await GetMaxCapacity();
            DateTime today = DateTime.Now;
            DateTime end = DateTime.Parse($"{today.AddDays(90).Day}/{today.AddDays(90).Month}/{today.AddDays(90).Year} 23:00");
            DateTime start;

            var servers = await _dbContext.FysiekeServers.Include(e => e.VirtualMachines).ThenInclude(e => e.Contract).ToListAsync();

            if (servers.Count == 0)
            {
                _logger.LogInformation("Request was successful");
                return new FysiekeServerResponse.GraphValues() { GraphData = max };
            }

            foreach (var _server in servers)
            {

                if (_server.VirtualMachines.Count == 0)
                {
                    continue;
                }


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
                        Hardware currentMax = new Hardware(maxHardware.Memory, maxHardware.Storage, maxHardware.Amount_vCPU);

                        for (int i = 0; i < end.Subtract(start).TotalDays; i++)
                        {
                            if (!max.ContainsKey(value))
                            {
                                max.Add(value, new Hardware(currentMax.Memory - _vm.Hardware.Memory, currentMax.Storage - _vm.Hardware.Storage, currentMax.Amount_vCPU - _vm.Hardware.Amount_vCPU));

                            }
                            else
                            {
                                if (max.TryGetValue(value, out var current))
                                {

                                    Hardware updatedHardware = new Hardware(current.Memory - _vm.Hardware.Memory, current.Storage - _vm.Hardware.Storage, current.Amount_vCPU - _vm.Hardware.Amount_vCPU);
                                    max[value] = updatedHardware;
                                }
                            }
                            value = value.AddDays(1);
                        }
                    }
                }
            }


            _logger.LogInformation("Request was successful");
            return new FysiekeServerResponse.GraphValues() { GraphData = max };
        }




        private async Task<Hardware> GetMaxCapacity()
        {
            _logger.LogInformation($"Request received: GetMaxCapacity()");
            Hardware maxHardware = new Hardware(0, 0, 0);
            var servers = await _dbContext.FysiekeServers.ToListAsync();

            foreach (var server in servers)
            {
                maxHardware.Memory += server.Hardware.Memory;
                maxHardware.Storage += server.Hardware.Storage;
                maxHardware.Amount_vCPU += server.Hardware.Amount_vCPU;
            }
            _logger.LogInformation("Request was successful");

            return maxHardware;

        }
    }
}


     