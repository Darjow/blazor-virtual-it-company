using Client.Extentions;
using Shared.Projects;
using System.Net.Http.Json;

namespace Client.Projects
{
    public class ProjectService : IProjectService
    {    

        private readonly HttpClient authenticatedClient;
        private const string endpoint = "api/project";

        public ProjectService(HttpClient authenticatedClient)
        {
            this.authenticatedClient = authenticatedClient;
        }
        public async Task<ProjectResponse.Create> CreateAsync(ProjectRequest.Create request)
        {
            var response =  await authenticatedClient.PostAsJsonAsync($"{endpoint}", request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<ProjectResponse.Create>();
        }

        public async Task<ProjectResponse.Delete> DeleteAsync(ProjectRequest.Delete request)
        { 
            var response = await authenticatedClient.DeleteAsync($"{endpoint}/{request}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<ProjectResponse.Delete>();
        }

        public async Task<ProjectResponse.Edit> EditAsync(ProjectRequest.Edit request)
        {
            var response = await authenticatedClient.PutAsJsonAsync($"{endpoint}", request);
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<ProjectResponse.Edit>();
        }
        
        public async Task<ProjectResponse.Detail> GetDetailAsync(ProjectRequest.Detail request)
        {
            var response =  await authenticatedClient.GetAsync($"{endpoint}/{request.Id}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<ProjectResponse.Detail>();
            
        }

        public async Task<ProjectResponse.All> GetIndexAsync(ProjectRequest.All request)
        {
            var qParams = request.GetQueryString();

            var response = await authenticatedClient.GetAsync($"{endpoint}/all?{qParams}");
            await response.HandleErrorResponse();
            return await response.Content.ReadFromJsonAsync<ProjectResponse.All>();
        }
    }
}
