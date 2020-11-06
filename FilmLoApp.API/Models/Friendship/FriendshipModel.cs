﻿using FilmLoApp.API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models.Friendship
{
    public class FriendshipModel
    {
        public long UserSenderId { get; set; }
        public UserModel UserSender { get; set; }

        public long UserRecipientId { get; set; }
        public UserModel UserRecipient { get; set; }

        public DateTime FriendshipDate { get; set; }

        public char StatusCodeID { get; set; }
       // public StatusCode StatusCode { get; set; }
    }
}
