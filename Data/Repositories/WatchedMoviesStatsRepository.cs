using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class WatchedMoviesStatsRepository : GenericRepository<WatchedMoviesStats>, IWatchedMoviesStatsRepository
    {
        public WatchedMoviesStatsRepository(DbContext context) : base(context)
        {

        }
    
    }
}
