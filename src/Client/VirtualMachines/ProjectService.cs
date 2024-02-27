using Client.Extentions;
using Shared.Projects;
using System.Net;
using System.Net.Http.Json;
/*
namespace Client.VirtualMachines
{
    public class ProjectService : IProjectenService
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private string endpoint = "api/project";


        public ProjectService(HttpClient _httpClient,*IHttpClientFactory _IHttpClientFactory)
        {
            this._httpClient = _httpClient;
            this._IHttpClientFactory = _IHttpClientFactory;


        }

        public Task<ProjectResponse.Create> CreateAsync(ProjectRequest.Create request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ProjectRequest.Delete request)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectResponse.Edit> EditAsync(ProjectRequest.Edit request)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectResponse.GetDetail> GetDetailAsync(ProjectRequest.GetDetail request)
        {
            var HttpClient = _IHttpClientFactory.CreateClient("AuthenticatedServerAPI");
            var queryParameters = request.ProjectenId;
            var response = await HttpClient.GetFromJsonAsync<ProjectenResponse.GetDetail>($"{endpoint}/{queryParameters}");
            return response;
        }

        public async Task<ProjectenResponse.GetIndex> GetIndexAsync(ProjectenRequest.GetIndex request)
        {
            var HttpClient = _IHttpClientFactory.CreateClient("AuthenticatedServerAPI");

            var queryParameters = request.GetQueryString();
            var response = await HttpClient.GetFromJsonAsync<ProjectenResponse.GetIndex>($"{endpoint}?{queryParameters}");
            return response;


        }
    }
}
*/