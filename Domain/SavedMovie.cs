using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain
{
  public class SavedMovie
    {

        public long UserId { get; set; }
        public User User { get; set; }
        public string MovieJMDBApiId { get; set; } 
        public MovieJMDBApi MovieJMDBApi { get; set; }
        [NotNull]
        public DateTime SavingDate { get; set; } // bice trenutni datum cuvanja filma
    }
}
