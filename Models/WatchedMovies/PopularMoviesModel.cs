using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WatchedMovies
{
    public class PopularMoviesModel
    {
        public long UserId { get; set; }
        public string MovieId { get; set; }
        public string MovieName { get; set; }
    }
}
