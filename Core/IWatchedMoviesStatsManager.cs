using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Core
{
    public interface IWatchedMoviesStatsManager
    {
        public WatchedMoviesStats GetWatchedMoviesStats(long currentUserId);
    }
}
