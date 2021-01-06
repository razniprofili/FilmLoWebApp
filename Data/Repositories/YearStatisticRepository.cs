using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class YearStatisticRepository : GenericRepository<YearStatistic>, IYearStatisticRepository
    {
        public YearStatisticRepository(DbContext context) : base(context)
        {

        }
    }
}
