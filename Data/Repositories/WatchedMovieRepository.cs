﻿using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class WatchedMovieRepository : GenericRepository<WatchedMovie>, IWatchedMovieRepository
    {
        public WatchedMovieRepository(DbContext context) : base(context)
        {

        }
    }
}
