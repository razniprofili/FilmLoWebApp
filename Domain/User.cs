using System;
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
        public List<WatchedMovie> WatchedMovies { get; set; }
        public List<SavedMovie> SavedMovies { get; set; }       
        public List<Friendship> FriendsSent { get; set; }        
        public List<Friendship> FriendsReceived { get; set; }
        public List<Notification> NotificationsSent { get; set; }
        public List<Notification> NotificationsReceived { get; set; }


        #region overridden methods
        public override bool Equals(object obj)
        {
            if (obj is User u)
            {
                return u.Id == this.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion

    }
}
