using FilmLoApp.API.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.SavedMovies
{
    public class SavedMovieModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public long UserId { get; set; }

        public UserModel User { get; set; }
    }
}
