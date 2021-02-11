using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Notification
{
    public class SendNotificationModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public long UserRecipientId { get; set; }
    }
}
