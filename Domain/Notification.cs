using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }
        public DateTime SendingDate { get; set; }
        public string Text { get; set; }

        public long UserSenderId { get; set; }
        [NotMapped]
        public User UserSender { get; set; }

        public long UserRecipientId { get; set; }
        [NotMapped]
        public User UserRecipient { get; set; }
    }
}
