using Common.ResourceParameters;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IFriendshipManager
    {
        public List<User> SearchMyFriends(long idUser, string searchCriteria);
        public object GetAllMyFriends(long idUser, ResourceParameters usersResourceParameters = null);
        public Friendship Add(Friendship friendship, long userId);
        public User GetFriendInfo(long idFriend, long idUser);
        public void DeleteFriend(long idFriend, long idUser);
        public Friendship AcceptRequest(long userId, long requestUserId);
        public void DeclineRequest(long userId, long requestUserId);
    }
}
