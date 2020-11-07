using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class MovieJMDBApiRepository : GenericRepository<MovieJMDBApi>, IMovieJMDBApiRepository
    {
        public MovieJMDBApiRepository(DbContext context) : base(context)
        {

        }
    }
}
