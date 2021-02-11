using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
   public class MovieJMDBApi
    {
        [Key]
        public string Id { get; set; } // id je oblika tt15784 u tom APIju
        public string Name { get; set; }
        public string Poster { get; set; }
        public List<SavedMovie> SavedUsers { get; set; }
        public List<WatchedMovie> WatchedUsers { get; set; }
        public MovieDetailsJMDBApi MovieDetailsJMDBApi { get; set; }
    }
}
