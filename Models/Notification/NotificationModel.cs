using Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Notification
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public DateTime SendingDate { get; set; }
        public string Text { get; set; }

        public long UserSenderId { get; set; }
        public UserModel UserSender { get; set; }

        public long UserRecipientId { get; set; }
        public UserModel UserRecipient { get; set; }
    }
}
