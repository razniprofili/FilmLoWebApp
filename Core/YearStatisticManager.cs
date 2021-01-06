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
        public List<YearStatistic> GetYearStatistic(long currentUserId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == currentUserId);
                ValidationHelper.ValidateNotNull(user);

                var yearStats = uow.YearStatisticRepository.Find(stat => stat.UserId == currentUserId);

                return yearStats.ToList();
            }
        }
    }
}
