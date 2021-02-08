using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.WatchedMovies
{
    public class AddWatchedMovieModel
    {
        [Required]
        public string Id { get; set; }
        public string Actors { get; set; }
        public int? Year { get; set; }
        [Required]
        public string Name { get; set; }
        public string Director { get; set; }
        public int? Duration { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public string Comment { get; set; }
        public string DateTimeWatched { get; set; }
        public string Poster { get; set; }
    }
}
