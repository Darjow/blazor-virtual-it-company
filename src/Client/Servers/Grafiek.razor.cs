using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Handlers;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using Domain.Common;
using Domain.Statistics.Datapoints;
using Domain.VirtualMachines.Statistics;
using Microsoft.AspNetCore.Components;
using Shared.VirtualMachines;
using System.Data;
using System.Drawing;
using Xamarin.Forms.Internals;

namespace Client.Servers
{
    public partial class Grafiek
    {
        [Inject] IVirtualMachineService VirtualMachineService { get; set; }
        [Parameter] public int Id { get; set; }

        private Dictionary<DateTime, Hardware> _data = new();
        private VirtualMachineDto.Rapportage vm;

        private bool Loading = false;
        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            await getVirtualmachine();
            vm.Statistics.GetFakeStatistics(StatisticsPeriod.DAILY).ForEach(e => _data.Add(e.Key, new Hardware(e.Value.HardWareInUse.Memory, e.Value.HardWareInUse.Storage, e.Value.HardWareInUse.Amount_vCPU)));
            Loading = false;
        }

        private async Task getVirtualmachine()
        {
            var response = await VirtualMachineService.RapporteringAsync(new VirtualMachineRequest.GetDetail { Id = Id });
            vm = response.VirtualMachine;
        }

    }

}
