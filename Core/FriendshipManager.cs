using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
   public class FriendshipManager
    {
        public List<User> SearchMyFriends(long idUser, string searchCriteria)
        {
            return null;
        }

        public List<User> LoadAllMyFriends(long idUser)
        {
            return null;
        }

        public Friendship Add(Friendship friendship)
        {
            using (var uow = new UnitOfWork())
            {
                // proveravamo da li postoje useri za kje se unosi prijateljstvo:

                var userSender = uow.UserRepository.FirstOrDefault(a => a.Id == friendship.UserSenderId);
                ValidationHelper.ValidateNotNull(userSender);

                var userRecipient = uow.UserRepository.FirstOrDefault(a => a.Id == friendship.UserRecipientId);
                ValidationHelper.ValidateNotNull(userRecipient);

                //proveravamo da li vec postoji prijateljstvo:

                var exist = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == friendship.UserSenderId && f.UserRecipientId == friendship.UserRecipientId);
                ValidationHelper.ValidateEntityExists(exist);

                //punimo preostalim podacima
                friendship.StatusCode = new StatusCode
                {
                    Code = 'R',
                    Name = "Requested"
                };
                friendship.StatusCodeID = 'R';
                friendship.FriendshipDate = DateTime.UtcNow;

                //dodajemo prijateljstvo
                uow.FriendshipRepository.Add(friendship);
                uow.Save();

                return friendship;

            }
               
        }

        public User GetFriendInfo(long idFriend, long idUser)
        {

            using (var uow = new UnitOfWork())
            {
                //prvo provera da li mi je prijatelj:
                var friendship = uow.FriendshipRepository.FirstOrDefault(user => user.UserSenderId == idUser && user.UserRecipientId == idFriend);
                ValidationHelper.ValidateNotNull(friendship);

                var myFriend = uow.UserRepository.FirstOrDefault(a => a.Id == idFriend);
                ValidationHelper.ValidateNotNull(myFriend);
                return myFriend;
            }
        }

        public void DeleteFriend(long idFriend, long idUser)
        {
            using (var uow = new UnitOfWork())
            {

                //prvo provera da li mi je prijatelj za svaki slucaj:
                var friendship = uow.FriendshipRepository.FirstOrDefault(user => user.UserSenderId == idUser && user.UserRecipientId == idFriend);
                ValidationHelper.ValidateNotNull(friendship);

                uow.FriendshipRepository.Delete(friendship);
                uow.Save();
            }
        }
    }
}
