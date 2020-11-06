﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.SavedMovies
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
    }
}