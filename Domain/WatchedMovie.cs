using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain
{
   public class WatchedMovie
    {
        //asocijativna klasa

        public long UserId { get; set; }
        public User User { get; set; }

        public long MovieDetailsJMDBApiId { get; set; }
        public MovieDetailsJMDBApi MovieDetailsJMDBApi { get; set; }

        [NotNull]
        public string WatchingDate { get; set; } // na front delu se bira datum, i mora da bude posebog formata, pa je lakse sacuvatikao string
        [NotNull]
        public int Rating { get; set; }
        [NotNull]
        public string Comment { get; set; }


    }
}
