using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class YearStatisticManager : IYearStatisticManager
    {
        #region Fields
        private readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public YearStatisticManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        public List<YearStatistic> GetYearStatistic(long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var yearStats = _uow.YearStatistic.Find(stat => stat.UserId == currentUserId, "");

            return yearStats.ToList();
        }
        #endregion

    }
}
