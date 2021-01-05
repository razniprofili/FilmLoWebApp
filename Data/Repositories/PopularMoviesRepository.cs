using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PopularMoviesRepository : GenericRepository<PopularMovies>, IPopularMoviesRepository
    {
        public PopularMoviesRepository(DbContext context) : base(context)
        {

        }

    }
}
