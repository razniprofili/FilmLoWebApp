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
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var friendships = uow.FriendshipRepository.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') || (m.UserRecipientId == idUser && m.StatusCodeID == 'A')); // ali mora i da bude prihvaceno prijateljstvo

                List<User> friends = new List<User>();

                foreach (var friend in friendships)
                {
                    if(friend.UserRecipientId == idUser)
                    {
                        var userFriend = uow.UserRepository.FirstOrDefault(f => f.Id == friend.UserSenderId && (f.Name.Contains(searchCriteria)  || f.Surname.Contains(searchCriteria) ));
                       
                        if(userFriend != null)
                            friends.Add(userFriend);
                    } 
                    else
                    {
                        var userFriend = uow.UserRepository.FirstOrDefault(f => f.Id == friend.UserRecipientId && (f.Name.Contains(searchCriteria) || f.Surname.Contains(searchCriteria) ));
                        
                        if (userFriend != null)
                            friends.Add(userFriend);
                    }
                }

                return friends;
            }
        }

        public List<User> GetAllMyFriends(long idUser)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var friendships = uow.FriendshipRepository.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') || (m.UserRecipientId == idUser && m.StatusCodeID == 'A'));

                List<User> friends = new List<User>();

                foreach (var friend in friendships)
                {
                    if (friend.UserSenderId == idUser)
                    {
                        var userFriend = uow.UserRepository.GetById(friend.UserRecipientId);
                        friends.Add(userFriend);
                    }
                    else
                    {
                        var userFriend = uow.UserRepository.GetById(friend.UserSenderId);
                        friends.Add(userFriend);
                    }
                }

                return friends;
            }
        }

        public Friendship Add(Friendship friendship, long userId)
        {
            using (var uow = new UnitOfWork())
            {
                // proveravamo da li postoje useri za kje se unosi prijateljstvo:

                var userSender = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(userSender);

                var userRecipient = uow.UserRepository.FirstOrDefault(a => a.Id == friendship.UserRecipientId);
                ValidationHelper.ValidateNotNull(userRecipient);

                //proveravamo da li vec postoji prijateljstvo:

                var exist = uow.FriendshipRepository.FirstOrDefault(f => (f.UserSenderId == userId && f.UserRecipientId == friendship.UserRecipientId) || (f.UserSenderId == friendship.UserRecipientId && f.UserRecipientId == userId));
                ValidationHelper.ValidateEntityExists(exist);


                friendship.StatusCodeID ='R';
                friendship.FriendshipDate = DateTime.Now;
                friendship.UserSender = userSender;
                friendship.UserRecipient = userRecipient;

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
                var friendship = uow.FriendshipRepository.FirstOrDefault(f => (f.UserSenderId == idUser && f.UserRecipientId == idFriend && f.StatusCodeID == 'A') || (f.UserSenderId == idFriend && f.UserRecipientId == idUser && f.StatusCodeID == 'A'));
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
                var friendship = uow.FriendshipRepository.FirstOrDefault(f => (f.UserSenderId == idUser && f.UserRecipientId == idFriend && f.StatusCodeID == 'A') || (f.UserSenderId == idFriend && f.UserRecipientId == idUser && f.StatusCodeID == 'A'));
                ValidationHelper.ValidateNotNull(friendship);

                uow.FriendshipRepository.Delete(friendship);
                uow.Save();
            }
        }

        public Friendship AcceptRequest(long userId, long requestUserId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji useri i prijateljstvo za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == requestUserId);
                ValidationHelper.ValidateNotNull(user);

                var friendship = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == requestUserId && f.UserRecipientId == userId && f.StatusCodeID == 'R'); // prihvata zahtev onaj ko je trenutno ulogovan tj on je recipient, i ako je zahtev u statusu Requested
                ValidationHelper.ValidateNotNull(friendship);

                //friendship.StatusCode = new StatusCode
                //{
                //    Code = 'A',
                //    Name = "Accepted"
                //};
                friendship.StatusCodeID = 'A';

                uow.FriendshipRepository.Update(friendship, requestUserId, userId);
                uow.Save();


                return friendship;
            }

        }

        public void DeclineRequest(long userId, long requestUserId) // kao delete metoda, obrisace friendship
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji useri i prijateljstvo za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == requestUserId);
                ValidationHelper.ValidateNotNull(user);

                var friendship = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == requestUserId && f.UserRecipientId == userId && f.StatusCodeID == 'R'); // prihvata zahtev onaj ko je trenutno ulogovan tj on je recipient
                ValidationHelper.ValidateNotNull(friendship);

                uow.FriendshipRepository.Delete(friendship);
                uow.Save();

            }
        }
    }
}
