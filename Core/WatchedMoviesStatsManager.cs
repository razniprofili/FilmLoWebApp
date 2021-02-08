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
        #region Fields
        private readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public WatchedMoviesStatsManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        public WatchedMoviesStats GetWatchedMoviesStats(long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var moviesStats = _uow.WatchedMoviesStats.FirstOrDefault(stat => stat.UserId == currentUserId, "");

            return moviesStats;
        }
        #endregion

    }
}
