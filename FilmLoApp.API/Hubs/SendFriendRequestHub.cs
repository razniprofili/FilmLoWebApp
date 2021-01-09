using Facade;
using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.SignalR;
using Models.Friendship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FilmLoApp.API.Hubs
{
    
    public class SendFriendRequestHub : Hub
    {
        private FilmLoFacade _facade;
        internal FilmLoFacade facade => _facade ?? (_facade = new FilmLoFacade());

        public async Task AddFriend(long currentUserId, AddFriendshipModel model)
        {
            var friendship = facade.Add(model, currentUserId);


            await Clients.Others.SendAsync("RequestReceived", friendship);
            await Clients.Caller.SendAsync("RequestSent");
        }

    }
}
