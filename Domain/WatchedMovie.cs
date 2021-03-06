﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain
{
   public class WatchedMovie
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public string MovieJMDBApiId { get; set; }
        public MovieJMDBApi MovieJMDBApi { get; set; }
        [NotNull]
        public string WatchingDate { get; set; }
        public DateTime DateTimeAdded { get; set; }
        [NotNull]
        public int Rating { get; set; }
        [NotNull]
        public string Comment { get; set; }
    }
}
