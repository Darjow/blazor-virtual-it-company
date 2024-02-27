using Domain.Common;
using Domain.Projecten;
using Domain.VirtualMachines.BackUp;
using Domain.VirtualMachines.Contract;
using Domain.VirtualMachines.VirtualMachine;
using Fakers.VirtualMachines;
using Shared.Servers;
using Shared.VirtualMachines;

namespace Services.VirtualMachines
{
    public class FakeVirtualMachineService : IVirtualMachineService
    {

        private List<VirtualMachine> _virtualMachines = new();

        public FakeVirtualMachineService()
        {
            _virtualMachines = VirtualMachineFaker.Instance.Generate(12);
        }

        public async Task<VirtualMachineResponse.Delete> DeleteAsync(VirtualMachineRequest.Delete request)
        {
            await Task.Delay(100);
            var p = _virtualMachines.SingleOrDefault(x => x.Id == request.Id);
            _virtualMachines.Remove(p);

            return new VirtualMachineResponse.Delete { Id = p.Id };
        }
        public async Task<VirtualMachineResponse.GetDetail> GetDetailAsync(VirtualMachineRequest.GetDetail request)
        {
            await Task.Delay(100);
            VirtualMachineResponse.GetDetail response = new();

          
            VirtualMachine? vm = _virtualMachines.SingleOrDefault(x => x.Id == request.Id);

            if (vm is not null)
            {
                response = new VirtualMachineResponse.GetDetail
                {
                    Id = vm.Id,
                    BackUp = vm.BackUp,
                    Contract = vm.Contract,
                    FysiekeServer = new FysiekeServerDto.Index { Hardware = vm.FysiekeServer.Hardware, HardWareAvailable = vm.FysiekeServer.HardwareAvailable, Id = vm.FysiekeServer.Id, Name = vm.FysiekeServer.Naam, ServerAddress = vm.FysiekeServer.ServerAddress},
                    Hardware = vm.Hardware,
                    Mode = vm.Mode,
                    Name = vm.Name,
                    OperatingSystem = vm.OperatingSystem,
                    VMConnection = new VMConnectionDto.Detail
                    {
                        FQDN = vm.Connection.FQDN, 
                        Hostname = vm.Connection.Hostname.ToString(), 
                        Password= vm.Connection.Password,
                        Username = vm.Connection.Username
                    }
                };

            }
            else
            {
                response= new VirtualMachineResponse.GetDetail
                {
                    Id = -1
                };
            }
            return response;
        }

        public async Task<VirtualMachineResponse.GetIndex> GetIndexAsync(VirtualMachineRequest.GetIndex request)
        {
            await Task.Delay(100);

            VirtualMachineResponse.GetIndex response = new();
            var query = _virtualMachines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(e => e.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            if (request.Status is not null)
            {
                query = query.Where(e => e.Mode.Equals(request.Status));

            }
            if (request.MaxMemory is not null)
            {
                query = query.Where(e => e.Hardware.Memory < request.MaxMemory);
            }
            if (request.MinMemory is not null)
            {
                query = query.Where(e => e.Hardware.Memory > request.MinMemory);

            }
            if (request.MinAmountCPU is not null)
            {
                query = query.Where(e => e.Hardware.Amount_vCPU > request.MinAmountCPU);

            }
            if (request.MaxAmountCPU is not null)
            {
                query = query.Where(e => e.Hardware.Amount_vCPU < request.MaxAmountCPU);

            }
            if (request.MinStorage is not null)
            {
                query = query.Where(e => e.Hardware.Storage > request.MinStorage);

            }
            if (request.MaxStorage is not null)
            {
                query = query.Where(e => e.Hardware.Storage < request.MaxStorage);

            }
            query.OrderBy(x => x.Name);



            response.TotalAmount = query.Count();
            response.VirtualMachines = query.Select(x => new VirtualMachineDto.Index
            {
                Id = x.Id,
                Name = x.Name,
                Mode = x.Mode
            }).ToList();

            return response;
        }
        public async Task<VirtualMachineResponse.Edit> EditAsync(VirtualMachineRequest.Edit request)
        {
            await Task.Delay(100);

            VirtualMachineResponse.Edit response = new();
            var vm = _virtualMachines.SingleOrDefault(x => x.Id == request.VirtualMachineId);

            var model = request.VirtualMachine;

            if (vm == null)
            {
                response.Id = -1;
            }
            else
            {
                var name = model.Name;
                var backup = model.Backup;

                vm.Name = name;
                vm.BackUp = backup;

                response.Id = vm.Id;
            }

            return response;
        }

        public async Task<VirtualMachineResponse.Create> CreateAsync(VirtualMachineRequest.Create request)
        {
            await Task.Delay(100);
            VirtualMachineResponse.Create response = new();

            var model = request.VirtualMachine;
            string name = model.Name;
            Backup backup = model.Backup;
            OperatingSystemEnum os = model.OperatingSystem;
            Hardware hw = model.Hardware;
            int p = model.ProjectId;

            int id = _virtualMachines.Max(x => x.Id) + 1;

            VirtualMachine vm = new VirtualMachine(name, os, hw, backup) { Id = id, Contract = new VMContract(request.CustomerId, id, model.Start, model.End) };

            _virtualMachines.Add(vm);

            response.Id = id;

            return response;

        }

        public async Task<VirtualMachineResponse.Rapport> RapporteringAsync(VirtualMachineRequest.GetDetail request)
        {
            await Task.Delay(100);
            VirtualMachineResponse.Rapport response = new();
            var vm = _virtualMachines.SingleOrDefault(x => x.Id == request.Id);
            VirtualMachineDto.Rapportage dto = new();
            dto.Statistics = vm.Statistics;
            dto.Id = vm.Id;
            dto.Name = vm.Name;
            response.VirtualMachine = dto;
            return response;

        }
    }
}