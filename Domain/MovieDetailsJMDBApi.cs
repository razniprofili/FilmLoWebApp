using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
   public class MovieDetailsJMDBApi
    {
        // nakon sto se pozove api za pretragu filma preko IDja, izbacuje dosta atributa
        // uzecu neke znacajnije

        [Key]
        public long Id { get; set; } // mi stavljamo nas id, a ne onaj tt8445
        public string Actors { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public int? Duration { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }

        // public int Rating { get; set; } u asocijativnu klasu, korisnikova ocena
        // public string Date { get; set; } u asocijativnu klasu
        // public string Comment { get; set; } u asocijativnu klasu, korisnikov komentar

        //Ima vise Usera, asocijativna veza
        public List<WatchedMovie> Users { get; set; } // mora ovako, a ne lista klase User
    }
}
