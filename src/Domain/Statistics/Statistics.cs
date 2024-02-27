using Domain.Common;
using Domain.Statistics.Datapoints;
using Domain.VirtualMachines.Statistics;
using System.Collections.Generic;
using System;

namespace Domain.Statistics;

public class Statistic
{



    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Hardware Hardware { get; set; }


    public Statistic(DateTime start, DateTime end, Hardware hardware)
    {
        StartTime = start;
        EndTime = end;
        Hardware = hardware;
    }
    public Statistic() { }


    public Dictionary<DateTime, DataPoint> GetFakeStatistics(StatisticsPeriod period)
    {
        DatapointsMock.Instance.Hardware = Hardware;
        List<DataPoint> _dataPoints = DatapointsMock.Instance.Generate(GetAmountOfTicks(period));

        List<DateTime> _datePoints = GetFakeDataPoints(period);

        Dictionary<DateTime, DataPoint> output = new();

        for (int i = 0; i < _dataPoints.Count; i++)
        {
            output.Add(_datePoints[i], _dataPoints[i]);
        }
        return output;

    }

    private List<DateTime> GetFakeDataPoints(StatisticsPeriod period)
    {
        List<DateTime> output = new();

        int max = GetAmountOfTicks(period);
        DateTime start = FormatStartTime(period);


        for (int i = 0; i < max; i++)
        {
            switch (period)
            {
                case StatisticsPeriod.HOURLY:
                    start = new DateTime(start.AddHours(1).Ticks);
                    output.Add(start);
                    break;

                case StatisticsPeriod.DAILY:
                    start = new DateTime(start.AddDays(1).Ticks);
                    output.Add(start);
                    break;

                case StatisticsPeriod.WEEKLY:
                    start = new DateTime(start.AddDays(7).Ticks);
                    output.Add(start);
                    break;

                case StatisticsPeriod.MONTHLY:
                    start = new DateTime(start.AddMonths(1).Ticks);
                    output.Add(start);
                    break;
            }

        }

        return output;

    }

    private DateTime FormatStartTime(StatisticsPeriod period)
    {

        int dayOfWeek = ((int)StartTime.DayOfWeek);

        switch (period)
        {
            case StatisticsPeriod.HOURLY:
                {
                    DateTime dt = StartTime.AddHours(1);
                    return DateTime.Parse($"{dt.Day}/{dt.Month}/{dt.Year} {dt.Hour}:00");
                }

            case StatisticsPeriod.DAILY:
                {
                    DateTime dt = StartTime.AddDays(1);
                    return DateTime.Parse($"{dt.Day}/{dt.Month}/{dt.Year} 00:00");

                }
            case StatisticsPeriod.WEEKLY:
                {
                    int check = 8;   // https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek?view=net-6.0
                    int addToCount = check - dayOfWeek; //begint vanaf week erna de maandag

                    DateTime dt = StartTime.AddDays(addToCount);
                    return DateTime.Parse($"{dt.Day}/{dt.Month}/{dt.Year} 00:00");
                }

            case StatisticsPeriod.MONTHLY:
                {
                    int check = DateTime.DaysInMonth(StartTime.Year, StartTime.Month);
                    int addToCount = check - StartTime.Day;

                    DateTime dt = StartTime.AddDays(addToCount);
                    return DateTime.Parse($"{dt.Day}/{dt.Month}/{dt.Year} 00:00");

                }

            default: throw new ArgumentException("No valid period provided");
        }
    }



    private int GetAmountOfTicks(StatisticsPeriod period)
    {
        switch (period)
        {
            case StatisticsPeriod.HOURLY: return (int)Math.Floor((DateTime.Now - StartTime).TotalHours);
            case StatisticsPeriod.DAILY: return (int)Math.Floor((DateTime.Now - StartTime).TotalDays);
            case StatisticsPeriod.WEEKLY: return (int)Math.Floor((DateTime.Now - StartTime).TotalDays % 7);
            case StatisticsPeriod.MONTHLY: return (int)Math.Floor((DateTime.Now - StartTime).TotalDays % 30);
        }

        return 0;
    }

}