using Shared.Servers;
using System.Net.Http.Json;

namespace Client.Servers
{
    public class FysiekeServerService : IFysiekeServerService
    {
        private readonly HttpClient authenticatedClient;
        private const string endpoint = "api/fysiekeserver";

        public FysiekeServerService(HttpClient authenticatedClient)
        {
            this.authenticatedClient = authenticatedClient;
        }

        public async Task<FysiekeServerResponse.Details> GetDetailsAsync(FysiekeServerRequest.Detail request)
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/{request.Id}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<FysiekeServerResponse.Details>();
        }

        public async Task<FysiekeServerResponse.Available> GetAllServers()
        {
            var response =  await authenticatedClient.GetAsync($"{endpoint}/all");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<FysiekeServerResponse.Available>();
        }

        public async Task<FysiekeServerResponse.ResourcesAvailable> GetAvailableHardWareOnDate(FysiekeServerRequest.Date date)
        {
            var response =  await authenticatedClient.PostAsJsonAsync($"{endpoint}/date", date);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<FysiekeServerResponse.ResourcesAvailable>();
        }

        public async Task<FysiekeServerResponse.GraphValues> GetGraphValueForServer()
        {
            var response = await authenticatedClient.GetAsync($"{endpoint}/graph");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<FysiekeServerResponse.GraphValues>();
        }
    }
}
