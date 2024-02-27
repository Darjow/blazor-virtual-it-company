using Shared.VirtualMachines;
using System.Linq;
using Persistence.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.VirtualMachines.VirtualMachine;
using System;
using Domain.Common;
using Domain.VirtualMachines.BackUp;
using Shared.Servers;
using Domain.Projecten;
using System.Diagnostics.Contracts;
using Domain.VirtualMachines.Contract;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Services.VirtualMachines
{
    public class VirtualMachineService : IVirtualMachineService
    {
        private readonly ILogger<VirtualMachineService> _logger;
        private readonly DotNetDbContext _dbContext;

        public VirtualMachineService(DotNetDbContext dbContext, ILogger<VirtualMachineService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            
        }



        public async Task<VirtualMachineResponse.Create> CreateAsync(VirtualMachineRequest.Create request)
        {
            _logger.LogInformation($"Request received: CreateAsync(...)");
            VirtualMachineResponse.Create response = new();

            var model = request.VirtualMachine;
            string name = model.Name;
            Backup backup = model.Backup;
            OperatingSystemEnum os = model.OperatingSystem;
            Hardware hw = model.Hardware;
            int projectId = model.ProjectId;
            int customerId = request.CustomerId;


            var project = await _dbContext.Projecten.FirstOrDefaultAsync(e => e.Id == projectId);

            if(project == null)
            {
                _logger.LogWarning($"Request failed: No project found with id: {request.VirtualMachine?.ProjectId}.");
                throw new EntityNotFoundException("Project", "virtualMachine.projectId", request.VirtualMachine?.ProjectId);

            }
            var vm = new VirtualMachine(name, os, hw, backup);
            var res = await _dbContext.VirtualMachines.AddAsync(vm);
            await _dbContext.SaveChangesAsync();

            var vmId = res.Entity.Id;
            vm.Contract = new VMContract(customerId, vm.Id, request.VirtualMachine.Start, request.VirtualMachine.End);
            project.VirtualMachines.Add(vm);
  

            await _dbContext.SaveChangesAsync();
            response.Id = vmId;

            _logger.LogInformation("Request was successful");
            return response;
        }

        public async Task DeleteAsync(VirtualMachineRequest.Delete request)
        {
            throw new NotImplementedException();
        }

        public async Task<VirtualMachineResponse.Edit> EditAsync(VirtualMachineRequest.Edit request)
        {
            throw new NotImplementedException();
        }


        public async Task<VirtualMachineResponse.GetDetail> GetDetailAsync(VirtualMachineRequest.GetDetail request)
        {
            _logger.LogInformation($"Request received: GetDetailsAsync({request.Id})");
            VirtualMachineResponse.GetDetail response = new();

            VirtualMachine? vm = await _dbContext.VirtualMachines
                .Include(e => e.BackUp)
                .Include(e => e.Connection)
                .Include(e => e.Contract)
                .Include(e => e.FysiekeServer)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if(vm == null)
            {
                _logger.LogWarning($"Request failed: No virtualMachine found with id: {request.Id}.");
                throw new EntityNotFoundException("VirtualMachine", "id", request.Id);

            }
            response = new VirtualMachineResponse.GetDetail
            {
                Id = vm.Id,
                BackUp = vm.BackUp,
                Contract = vm.Contract,
                Hardware = vm.Hardware, 
                FysiekeServer = new FysiekeServerDto.Index { Hardware = vm?.FysiekeServer?.Hardware, HardWareAvailable = vm.FysiekeServer?.HardwareAvailable, Id = (vm.FysiekeServer == null? 0 : vm.FysiekeServer.Id), Name = vm.FysiekeServer?.Naam, ServerAddress = vm.FysiekeServer?.ServerAddress },
                Mode = vm.Mode,
                Name = vm.Name,
                OperatingSystem = vm.OperatingSystem, 
                VMConnection =  vm.Connection == null? null: new VMConnectionDto.Detail
                {
                    FQDN = vm.Connection?.FQDN,
                    Hostname = vm.Connection?.Hostname?.ToString(),
                    Password = vm.Connection?.Password,
                    Username = vm.Connection?.Username
                }
            };

            _logger.LogInformation("Request was successful");

            return response;

        }

        public Task<VirtualMachineResponse.GetIndex> GetIndexAsync(VirtualMachineRequest.GetIndex request)
        {
            throw new NotImplementedException();
        }

        Task<VirtualMachineResponse.Delete> IVirtualMachineService.DeleteAsync(VirtualMachineRequest.Delete request)
        {
            throw new NotImplementedException();
        }

        public async Task<VirtualMachineResponse.Rapport> RapporteringAsync(VirtualMachineRequest.GetDetail request)
        {
            _logger.LogInformation($"Request received: RapporteringAsync({request.Id})");

            VirtualMachineResponse.Rapport response = new();
            var vm = await _dbContext.VirtualMachines.Include(e => e.Contract).SingleOrDefaultAsync(x => x.Id == request.Id);
            VirtualMachineDto.Rapportage dto = new();
            dto.Statistics = new Domain.Statistics.Statistic(vm.Contract.StartDate, vm.Contract.EndDate, vm.Hardware);
            dto.Id = vm.Id;
            dto.Name = vm.Name;
            response.VirtualMachine = dto;
            _logger.LogInformation("Request was successful");

            return response;
        }
    }
}