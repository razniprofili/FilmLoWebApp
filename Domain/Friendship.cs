using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
   public class Friendship
    {
        public long UserSenderId { get; set; }
        [NotMapped]
        public User UserSender { get; set; }

        public long UserRecipientId { get; set; }
        [NotMapped]
        public User UserRecipient { get; set; }

        public DateTime FriendshipDate { get; set; }

        public char StatusCodeID { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
