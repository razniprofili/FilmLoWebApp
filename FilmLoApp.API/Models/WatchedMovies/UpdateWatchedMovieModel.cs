﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.WatchedMovies
{
    public class UpdateWatchedMovieModel
    {

        public int Rate { get; set; }
        public string Comment { get; set; }
        public string DateTimeWatched { get; set; }

    }
}
