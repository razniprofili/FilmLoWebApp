using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.Friendship
{
    public class AddFriendshipModel
    {
        //[Required]
        //public long UserSenderId { get; set; }
        [Required]
        public long UserRecipientId { get; set; }
      
    }
}
