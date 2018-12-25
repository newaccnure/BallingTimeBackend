using System;

namespace BallingTimeBackend.Data_for_frontend
{
    public class UserStats
    {
        public double AverageSpeed { get; set; }
        public double AverageRepsPerSec { get; set; }
        public double AverageAccuracy { get; set; }
        public DateTime PracticeDay { get; set; }
    }
}
