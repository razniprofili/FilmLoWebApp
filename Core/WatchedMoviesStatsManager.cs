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
        public WatchedMoviesStats GetWatchedMoviesStats(long currentUserId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == currentUserId);
                ValidationHelper.ValidateNotNull(user);

                var moviesStats = uow.WatchedMoviesStatsRepository.FirstOrDefault(stat => stat.UserId == currentUserId);

                return moviesStats;
            }
        }
    }
}
