using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WatchedMovies
{
    public class YearStatisticModel
    {
        public long UserId { get; set; }
        public string Year { get; set; }
        public int Count { get; set; }
    }
}
