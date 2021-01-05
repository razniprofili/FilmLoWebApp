using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class WatchedMoviesStats
    {
        public long UserId { get; set; }
        public int TotalCount { get; set; }
        public int TotalTime { get; set; }
        public double AverageRate { get; set; }
    }
}
