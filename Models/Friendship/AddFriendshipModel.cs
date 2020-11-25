using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Friendship
{
   public class AddFriendshipModel
    {
        //[Required]
        //public long UserSenderId { get; set; }
        [Required]
        public long UserRecipientId { get; set; }
    }
}
