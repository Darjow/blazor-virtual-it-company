using Client.Extentions;
using Shared.Projects;
using Shared.VirtualMachines;
using System.Net.Http.Json;

namespace Client.VirtualMachines
{

    public class VirtualMachineService : IVirtualMachineService
    {

        private readonly HttpClient authenticatedClient;

        private const string endpoint = "api/virtualmachine";


        public VirtualMachineService(HttpClient client)
        {
            this.authenticatedClient = client;
        }

        public Task<VirtualMachineResponse.GetIndex> GetIndexAsync(VirtualMachineRequest.GetIndex request)
        {
            throw new NotImplementedException();
        }

        public async Task<VirtualMachineResponse.GetDetail> GetDetailAsync(VirtualMachineRequest.GetDetail request)
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/{request.Id}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<VirtualMachineResponse.GetDetail>();
        }

        public Task<VirtualMachineResponse.Delete> DeleteAsync(VirtualMachineRequest.Delete request)
        {
            throw new NotImplementedException();
        }

        public async Task<VirtualMachineResponse.Create> CreateAsync(VirtualMachineRequest.Create request)
        {
            var response = await authenticatedClient.PostAsJsonAsync(endpoint, request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<VirtualMachineResponse.Create>();
        }

        public Task<VirtualMachineResponse.Edit> EditAsync(VirtualMachineRequest.Edit request)
        {
            throw new NotImplementedException();
        }

        public async Task<VirtualMachineResponse.Rapport> RapporteringAsync(VirtualMachineRequest.GetDetail request)
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/graph/{request.Id}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<VirtualMachineResponse.Rapport>();
        }
    }
}
