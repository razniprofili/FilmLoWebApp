using FilmLoApp.API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.WatchedMovies
{
    public class WatchedMovieModel // ovo mi vracamo kljentu!
    {
        public string Id { get; set; }
        public string Actors { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public int? Duration { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string DateTimeWatched { get; set; }
        public long UserId { get; set; }
        public UserModel User { get; set; }
    }
}
