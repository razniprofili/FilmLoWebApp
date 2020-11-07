﻿using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class MovieDetailsJMDBApiRepository : GenericRepository<MovieDetailsJMDBApi>, IMovieDetailsJMDBApiRepository
    {
        public MovieDetailsJMDBApiRepository(DbContext context) : base(context)
        {

        }
    }
}
