using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IYearStatisticManager
    {
        public List<YearStatistic> GetYearStatistic(long currentUserId);
    }
}
