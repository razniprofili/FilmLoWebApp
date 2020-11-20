using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    // nakon pretrage JMDB API-a, taj API nam izbacuje samo ove podatke o filmu, pa cemo njih sacuvati
    // ovo je film koji se cuva (saved movie), za gledanje kasnije
    // Dovoljni su nam ovi podaci
   public class MovieJMDBApi
    {
        [Key]
        public string Id { get; set; } // id je oblika tt15784 u tom APIju
        public string Name { get; set; }
        public string Poster { get; set; }
        //Ima vise Usera, asocijativna veza
        public List<SavedMovie> SavedUsers { get; set; } // mora ovako a ne lista klase User
        public List<WatchedMovie> WatchedUsers { get; set; }
        public MovieDetailsJMDBApi MovieDetailsJMDBApi { get; set; }
    }
}
