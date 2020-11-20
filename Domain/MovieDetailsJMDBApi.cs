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
        public string Actors { get; set; }
        public int? Year { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public int? Duration { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
    }
}
