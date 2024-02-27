using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Servers
{
    public interface IFysiekeServerService
    {
        public Task<FysiekeServerResponse.Details> GetDetailsAsync(FysiekeServerRequest.Detail request);
        public Task<FysiekeServerResponse.Available> GetAllServers();
        public Task<FysiekeServerResponse.ResourcesAvailable> GetAvailableHardWareOnDate(FysiekeServerRequest.Date date);
        public Task<FysiekeServerResponse.GraphValues> GetGraphValueForServer();

    }
}