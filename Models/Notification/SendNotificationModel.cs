using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Notification
{
    public class SendNotificationModel
    {

        public string Text { get; set; }

        public long UserRecipientId { get; set; }
    }
}
