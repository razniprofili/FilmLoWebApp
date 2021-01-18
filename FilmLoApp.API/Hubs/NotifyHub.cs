using Facade;
using Microsoft.AspNetCore.SignalR;
using Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Hubs
{
    public class NotifyHub : Hub
    {
        private FilmLoFacade _facade;
        internal FilmLoFacade facade => _facade ?? (_facade = new FilmLoFacade());

        public async Task NotifyFriend(long currentUserId, SendNotificationModel notificationModel)
        {
            var notification = facade.SendNotification(notificationModel, currentUserId);

            await Clients.Others.SendAsync("NotificationReceived", notification);
            await Clients.Caller.SendAsync("NotificationSent");
        }
    }
}
