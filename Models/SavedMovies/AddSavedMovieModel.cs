using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.SavedMovies
{
    public class AddSavedMovieModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public long UserId { get; set; }

        //for movie details:

        [Required]
        public string Actors { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public int? Duration { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Country { get; set; }


    }
}
