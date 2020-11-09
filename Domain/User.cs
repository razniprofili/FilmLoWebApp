﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Picture { get; set; }

        //public List<MovieDetailsJMDBApi> WatchedMovies { get; set; }
        public List<WatchedMovie> WatchedMovies { get; set; }
        // public List<MovieJMDBApi> SavedMovies { get; set; }
        public List<SavedMovie> SavedMovies { get; set; }
        
        public List<Friendship> FriendsSent { get; set; }
        
        public List<Friendship> FriendsReceived { get; set; }
   
    }
}
