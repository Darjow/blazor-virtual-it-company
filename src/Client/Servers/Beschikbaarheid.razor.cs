using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using Domain.Common;
using Microsoft.AspNetCore.Components;
using Shared.Servers;
using System.Drawing;
using ChartJs.Blazor;
using Xamarin.Forms.Internals;

namespace Client.Servers
{
    public partial class Beschikbaarheid
    {
        [Inject] public IFysiekeServerService FysiekeServerService { get; set; }

        private List<FysiekeServerDto.Beschikbaarheid> Servers { get; set; }
        private Dictionary<DateTime, Hardware> _data = new();


        private DateTime DateStart { get; set; } = DateTime.Now;
        private DateTime DateEnd { get; set; } = DateTime.Now;


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetAvailableResources();
        }

        private async Task GetAvailableResources()
        {
            loading = true;
            var response = await FysiekeServerService.GetAvailableHardWareOnDate(new FysiekeServerRequest.Date() { FromDate = DateStart, ToDate = DateEnd });
            Servers = response.Servers;


            loading = false;
        }

        private async Task GetAvailableResourcesTotal()
        {
            loading = true;
            var response = await FysiekeServerService.GetGraphValueForServer();
            _data = response.GraphData;
            loading = false;
        }
    }
}
