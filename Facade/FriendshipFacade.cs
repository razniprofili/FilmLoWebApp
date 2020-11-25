using Domain;
using Models.Friendship;
using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public partial class FilmLoFacade
    {
        public UserModel GetFriendInfo (long friendId, long currentUserId)
        {
            var user = FriendshipManager.GetFriendInfo(friendId, currentUserId);
            return Mapper.Mapper.AutoMap<User, UserModel>(user);
        }

        public void DeleteFriend(long friendId, long currentUserId)
        {
            FriendshipManager.DeleteFriend(friendId, currentUserId);
        }

        public List<UserModel> GetAllMyFriends(long currentUserId)
        {
            List<User> friends = FriendshipManager.GetAllMyFriends(currentUserId);
            return friends.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        public List<UserModel> SearchMyFriends(long currentUserId, string criteria)
        {
            var friends = FriendshipManager.SearchMyFriends(currentUserId, criteria);
            return friends.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        public FriendshipModel Add(AddFriendshipModel model, long currentUserId)
        {
            var friendship = FriendshipManager.Add(Mapper.Mapper.Map(model, currentUserId), currentUserId);
            return Mapper.Mapper.Map(friendship);
        }

        public FriendshipModel AcceptRequest(long currentUserId, long friendId)
        {
            var acceptedRequest = FriendshipManager.AcceptRequest(currentUserId, friendId);
            return Mapper.Mapper.Map(acceptedRequest);
        }

        public void DeclineRequest(long currentUserId, long friendId)
        {
            FriendshipManager.DeclineRequest(currentUserId, friendId);
        }
    }
}
