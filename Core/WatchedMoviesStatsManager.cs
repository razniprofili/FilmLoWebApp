using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class WatchedMoviesStatsManager : IWatchedMoviesStatsManager
    {
        private readonly IUnitOfWork _uow;
        public WatchedMoviesStatsManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public WatchedMoviesStats GetWatchedMoviesStats(long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var moviesStats = _uow.WatchedMoviesStats.FirstOrDefault(stat => stat.UserId == currentUserId, "");

            return moviesStats;
        }
    }
}
