using Core;
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

        internal readonly IFriendshipManager _friendshipManager;
        internal readonly IUserManager _userManager;

        public SendFriendRequestHub(IUserManager userManager, IFriendshipManager friendshipManager)
        {
            _userManager = userManager;
            _friendshipManager = friendshipManager;
        }

        private FilmLoFacade _facade;
        internal FilmLoFacade facade => _facade ?? (_facade = new FilmLoFacade(_userManager, _friendshipManager));

        public async Task AddFriend(long currentUserId, AddFriendshipModel model)
        {
            var friendship = facade.Add(model, currentUserId);


            await Clients.Others.SendAsync("RequestReceived", friendship);
            await Clients.Caller.SendAsync("RequestSent");
        }

    }
}
