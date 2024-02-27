using ChartJs.Blazor;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using Domain.Common;
using System.Drawing;

namespace Client.Servers.Component
{
    public partial class Graph
    {
        [Parameter] public Dictionary<DateTime, Hardware> Data { get; set; }

        private LineConfig _config { get; set; }
        private Chart _ref { get; set; }


        protected override async Task OnInitializedAsync()
        {
            ConfigureLineConfig();
            AddLabels();
            AddDataToDataSet();
        }


        private void ConfigureLineConfig()
        {

            _config = new LineConfig
            {
                Options = new LineOptions
                {
                    Legend = new Legend()
                    {
                        Labels = new LegendLabels()
                        {
                            Padding = 10,
                        }
                    },
                    Responsive = true,

                    Tooltips = new Tooltips
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Hover = new Hover
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Scales = new Scales
                    {
                        XAxes = new List<CartesianAxis>
                    {
                        new CategoryAxis
                        {
                            ScaleLabel = new ScaleLabel
                            {
                                LabelString = "Month"
                            }
                        }
                    },
                        YAxes = new List<CartesianAxis>
                    {
                        new LinearCartesianAxis
                        {
                            ScaleLabel = new ScaleLabel
                            {
                                LabelString = "Value"
                            }
                        },
                    }
                    },

                }
            };
        }


        public void AddDataToDataSet()
        {

            IDataset<int> memory = new LineDataset<int>()
            {
                Label = "RAM (GB)",
                BorderColor = ColorUtil.FromDrawingColor(Color.DarkRed),
            };
            IDataset<int> storage = new LineDataset<int>()
            {
                Label = "Opslag (GB)",
                BorderColor = ColorUtil.FromDrawingColor(Color.DarkBlue),
            }; IDataset<int> cores = new LineDataset<int>()
            {
                Label = "#Cores",
                BorderColor = ColorUtil.FromDrawingColor(Color.DarkGreen),
            };

            _config.Data.Datasets.Add(memory);
            _config.Data.Datasets.Add(storage);
            _config.Data.Datasets.Add(cores);


            foreach (LineDataset<int> dataSet in _config.Data.Datasets)
            {
                switch (dataSet.Label)
                {
                    case "RAM (GB)":
                        {
                            dataSet.AddRange(Data.Select(e => (e.Value.Memory / 1000)));
                            break;
                        }
                    case "Opslag (GB)":
                        {
                            dataSet.AddRange(Data.Select(e => e.Value.Storage / 1000));
                            break;
                        }
                    case "#Cores":
                        {
                            dataSet.AddRange(Data.Select(e => e.Value.Amount_vCPU));
                            break;
                        }
                }
            }
        }

        private void AddLabels()
        {

            DateTime min = Data.Keys.First();
            DateTime max = Data.Keys.Last();

            int difference = max.Subtract(min).Days;
            _config.Data.Labels.Add(min.ToString("dd/MM/yyy"));

            if (difference <= 8)
            {
                for (int i = 1; i <= difference; i++)
                {
                    min = min.AddDays(1);
                    _config.Data.Labels.Add(min.ToString("dd/MM/yyy"));
                }
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    min = min.AddDays(Math.Floor(difference / 8.0));
                    _config.Data.Labels.Add(min.ToString("dd/MM/yyy"));
                }
            }


            _config.Data.Labels.Add(max.ToString("dd/MM/yyyy"));

        }
    }
}