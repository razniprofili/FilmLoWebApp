using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain
{
  public class SavedMovie
    {
        //asocijativna klasa iz veze vise-vise
        // pisem je jer pamti i datum

        public long UserId { get; set; }
        public User User { get; set; }
        public string MovieJMDBApiId { get; set; } // jer je oblika tt515874
        public MovieJMDBApi MovieJMDBApi { get; set; }
        [NotNull]
        public DateTime SavingDate { get; set; } // bice trenutni datum cuvanja filma
    }
}
