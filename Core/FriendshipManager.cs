using Common.Exceptions;
using Common.Helpers;
using Common.ResourceParameters;
using Core.Services;
using Data;
using Domain;
using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class FriendshipManager : IFriendshipManager
    {

        #region PrivateFields

        //private readonly IUnitOfWork _uow;

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _servicePropertyChecker;

        #endregion

        #region Constructors
        public FriendshipManager(IPropertyMappingService propertyMappingService, IPropertyCheckerService checker)
        {
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));

            _servicePropertyChecker = checker ??
               throw new ArgumentNullException(nameof(checker));
        }

        //public UserManager(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //}

        #endregion

        #region Methods

        public List<User> SearchMyFriends(long idUser, string searchCriteria)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var friendships = uow.FriendshipRepository.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') || (m.UserRecipientId == idUser && m.StatusCodeID == 'A')).ToList(); // ali mora i da bude prihvaceno prijateljstvo

                List<User> friends = new List<User>();

                foreach (var friend in friendships)
                {
                    if (friend.UserRecipientId == idUser)
                    {
                        var userFriend = uow.UserRepository.FirstOrDefault(f => f.Id == friend.UserSenderId && (f.Name.Contains(searchCriteria) || f.Surname.Contains(searchCriteria)));

                        if (userFriend != null)
                            friends.Add(userFriend);
                    }
                    else
                    {
                        var userFriend = uow.UserRepository.FirstOrDefault(f => f.Id == friend.UserRecipientId && (f.Name.Contains(searchCriteria) || f.Surname.Contains(searchCriteria)));

                        if (userFriend != null)
                            friends.Add(userFriend);
                    }
                }

                return friends;
            }
        }


        public object GetAllMyFriends(long idUser, ResourceParameters usersResourceParameters = null)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                if (usersResourceParameters != null)
                {
                    // provera da li postoje polja za sort
                    if (!_propertyMappingService.ValidMappingExistsFor<UserModel, User>
                    (usersResourceParameters.OrderBy))
                    {
                        throw new ValidationException($"{usersResourceParameters.OrderBy} fields for ordering do not exist!");
                    }

                    //provera da li postoji properti za data shaping
                    if (!_servicePropertyChecker.TypeHasProperties<UserModel>
                      (usersResourceParameters.Fields))
                    {
                        throw new ValidationException($"{usersResourceParameters.Fields} fields for shaping do not exist!");
                    }
                }

                var friendships = uow.FriendshipRepository.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') || (m.UserRecipientId == idUser && m.StatusCodeID == 'A')).ToList();

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

                if (usersResourceParameters != null)
                    return generateResult(friends, usersResourceParameters);
                else
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


                friendship.StatusCodeID = 'R';
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

        #endregion

        #region Private Methods

        private PagedList<User> generateResult(List<User> friends, ResourceParameters usersResourceParameters)
        {
            IQueryable<User> fr = friends.AsQueryable<User>();

            if (!string.IsNullOrWhiteSpace(usersResourceParameters.SearchQuery))
            {
                var searchQuery = usersResourceParameters.SearchQuery.Trim().ToLower();
                fr = fr.Where(a => a.Name.ToLower().Contains(searchQuery) || a.Surname.ToLower().Contains(searchQuery)); // ako je pocetno slovo veliko T npr, a mi unesemo t
            }

            if (!string.IsNullOrWhiteSpace(usersResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var userPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<UserModel, User>();

                fr = fr.ApplySort(usersResourceParameters.OrderBy,
                    userPropertyMappingDictionary);
            }

            return PagedList<User>.Create(fr, usersResourceParameters.PageNumber, usersResourceParameters.PageSize);
        }

        #endregion

    }
}
