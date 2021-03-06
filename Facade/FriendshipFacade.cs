﻿using Common.Helpers;
using Common.ResourceParameters;
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
        public FriendshipModel GetFriendInfo (long friendId, long currentUserId)
        {
            var friendInfo = FriendshipManager.GetFriendInfo(friendId, currentUserId);
            return Mapper.Mapper.Map(friendInfo);
        }

        public void DeleteFriend(long friendId, long currentUserId)
        {
            FriendshipManager.DeleteFriend(friendId, currentUserId);
        }

        public List<UserModel> GetAllMyFriends(long currentUserId)
        {
            List<User> friends = FriendshipManager.GetAllMyFriends(currentUserId) as List<User>;
            return friends.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        public PagedList<User> GetAllMyFriends(long currentUserId, ResourceParameters usersResourceParameters)
        {
            return FriendshipManager.GetAllMyFriends(currentUserId, usersResourceParameters) as PagedList<User>;
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

        public List<FriendshipModel> FriendRequests(long currentUserId)
        {
            var friendRequests = FriendshipManager.FriendRequests(currentUserId);

            return friendRequests.Select(request => Mapper.Mapper.Map(request)).ToList();
        }


        public List<FriendshipModel> SentFriendRequests(long currentUserId)
        {
            var sentRequests = FriendshipManager.SentFriendRequests(currentUserId);

            return sentRequests.Select(request => Mapper.Mapper.Map(request)).ToList();
        }

        public List<UserModel> MutualFriends(long currentUserId, long userId)
        {
            List<User> mutualFriends = FriendshipManager.MutualFriends(currentUserId, userId) as List<User>;

            return mutualFriends.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

    }
}
