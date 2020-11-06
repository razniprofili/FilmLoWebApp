using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.WatchedMovies
{
    public class AddWatchedMovieModel
    {
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
        //[Required]
        //public int UserId { get; set; } // imacu promenljivu koja prati trenutno ulogovanog usera, mozda nam ovo nije neophodno, ili mozemo u ruti da damo id Usera
    }
}
