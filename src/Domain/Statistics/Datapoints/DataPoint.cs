using Domain.Common;


namespace Domain.Statistics.Datapoints
{
    public class DataPoint
    {
        public int Tick { get; set; }
        public Hardware HardWareInUse { get; set; }

        public DataPoint(int tick, Hardware hardware_in_use)
        {
            Tick = tick;
            HardWareInUse = hardware_in_use;

        }

    }
}