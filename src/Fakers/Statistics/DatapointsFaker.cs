using Bogus;
using Domain.Common;
using Domain.Statistics.Datapoints;
using System;
using System.Security.Cryptography;

namespace Fakers.Statistics
{

    public class DatapointsFaker : Faker<DataPoint>
    {

        //singleton zodat hij niet steeds een nieuwe faker object moet creëren per virtual machine (memory heap)
        private static DatapointsFaker _instance;

        public static DatapointsFaker Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new DatapointsFaker();
                }
                return _instance;
            }
        }

        public int Tick { get; set; }

        public Hardware Hardware { get { return _hardware; } set { _hardware = value; Tick = 0; } }
        private Hardware _hardware;


        public DatapointsFaker()
        {
            CustomInstantiator(e => new DataPoint(Tick++, GenerateRandomHardWareUsage()));
        }




        public Hardware GenerateRandomHardWareUsage()
        {
            int m = (int)Math.Ceiling(Hardware.Memory * GetDouble(0.7, 1.0));
            int s = (int)Math.Ceiling(Hardware.Storage * GetDouble(0.1, 0.3));
            int c = (int)Math.Ceiling(Hardware.Amount_vCPU * GetDouble(0.5, 0.9));

            return new Hardware(m < 1000 ? m < 100 ? m * 10 : m * 2 : m, s < 1000 ? s * 3 : s, c == 0 ? RandomNumberGenerator.GetInt32(4) == 1 ? 1 : 0 : c);
        }

        private double GetDouble(double min, double max)
        {
            return new Random().NextDouble() * (max - min) + min;

        }


    }
}