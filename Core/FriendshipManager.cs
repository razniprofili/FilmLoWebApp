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

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _servicePropertyChecker;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Constructors

        public FriendshipManager(IPropertyMappingService propertyMappingService, IPropertyCheckerService checker,
            IUnitOfWork uow)
        {
            _propertyMappingService = propertyMappingService;
            _servicePropertyChecker = checker;
            _uow = uow;
        }


        #endregion

        #region Methods

        public List<User> SearchMyFriends(long idUser, string searchCriteria)
        {
            //using (var uow = new UnitOfWork())
            //{
                //provera da li postoji user za svaki slucaj:
                var user = _uow.Users.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var friendships = _uow.Friendships.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') 
                || (m.UserRecipientId == idUser && m.StatusCodeID == 'A')).ToList(); // ali mora i da bude prihvaceno prijateljstvo

                List<User> friends = new List<User>();

                foreach (var friend in friendships)
                {
                    if (friend.UserRecipientId == idUser)
                    {
                        var userFriend = _uow.Users.FirstOrDefault(f => f.Id == friend.UserSenderId 
                        && (f.Name.Contains(searchCriteria) 
                        || f.Surname.Contains(searchCriteria)));

                        if (userFriend != null)
                            friends.Add(userFriend);
                    }
                    else
                    {
                        var userFriend = _uow.Users.FirstOrDefault(f => f.Id == friend.UserRecipientId 
                        && (f.Name.Contains(searchCriteria) || f.Surname.Contains(searchCriteria)));

                        if (userFriend != null)
                            friends.Add(userFriend);
                    }
                }

                return friends;
           // }
        }

        public object GetAllMyFriends(long idUser, ResourceParameters usersResourceParameters = null)
        {

            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == idUser, "");
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

            var friendships = _uow.Friendships.Find(m => (m.UserSenderId == idUser && m.StatusCodeID == 'A') 
            || (m.UserRecipientId == idUser && m.StatusCodeID == 'A'), "").ToList();

            List<User> friends = new List<User>();


            foreach (var friend in friendships)
            {
                if (friend.UserSenderId == idUser)
                {
                    var userFriend = _uow.Users.GetById(friend.UserRecipientId);
                    friends.Add(userFriend);

                }
                else
                {
                    var userFriend = _uow.Users.GetById(friend.UserSenderId);
                    friends.Add(userFriend);
                }
            }

            if (usersResourceParameters != null)
                return generateResult(friends, usersResourceParameters);
            else
                return friends;
                
        }

        public Friendship Add(Friendship friendship, long userId)
        {
            // proveravamo da li postoje useri za kje se unosi prijateljstvo:
            var userSender = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(userSender);

            var userRecipient = _uow.Users.FirstOrDefault(a => a.Id == friendship.UserRecipientId, "");
            ValidationHelper.ValidateNotNull(userRecipient);

            //proveravamo da li vec postoji prijateljstvo:

            var exist = _uow.Friendships.FirstOrDefault(f => (f.UserSenderId == userId && f.UserRecipientId == friendship.UserRecipientId) 
            || (f.UserSenderId == friendship.UserRecipientId && f.UserRecipientId == userId), "");
            ValidationHelper.ValidateEntityExists(exist);


            friendship.StatusCodeID = 'R';
            friendship.FriendshipDate = DateTime.Now;
            friendship.UserSender = userSender;
            friendship.UserRecipient = userRecipient;

            //dodajemo prijateljstvo
            _uow.Friendships.Add(friendship, "");
            _uow.Save();

            return friendship;
        }

        public Friendship GetFriendInfo(long idFriend, long idUser)
        {

            //prvo provera da li mi je prijatelj:
            var friendship = _uow.Friendships.FirstOrDefault(f => (f.UserSenderId == idUser && f.UserRecipientId == idFriend && f.StatusCodeID == 'A') || (f.UserSenderId == idFriend && f.UserRecipientId == idUser && f.StatusCodeID == 'A'));
            ValidationHelper.ValidateNotNull(friendship);

            var myFriend = _uow.Users.FirstOrDefault(a => a.Id == idFriend);
            ValidationHelper.ValidateNotNull(myFriend);

            var userSender = _uow.Users.FirstOrDefault(a => a.Id == idUser);
            ValidationHelper.ValidateNotNull(userSender);

            if(friendship.UserSenderId == idUser)
                friendship.UserSender = userSender;
            else
                friendship.UserSender = myFriend;

            if (friendship.UserRecipientId == idFriend)
                friendship.UserRecipient = myFriend;
            else
                friendship.UserRecipient = userSender;

            return friendship;
        }

        public void DeleteFriend(long idFriend, long idUser)
        {
            //prvo provera da li mi je prijatelj za svaki slucaj:
            var friendship = _uow.Friendships.FirstOrDefault(f => (f.UserSenderId == idUser && f.UserRecipientId == idFriend && f.StatusCodeID == 'A') 
            || (f.UserSenderId == idFriend && f.UserRecipientId == idUser && f.StatusCodeID == 'A'), "");
            ValidationHelper.ValidateNotNull(friendship);

            _uow.Friendships.Delete(friendship);
            _uow.Save();
        }

        public Friendship AcceptRequest(long userId, long requestUserId)
        {
            //provera da li postoji useri i prijateljstvo za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var userFriend = _uow.Users.FirstOrDefault(a => a.Id == requestUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var friendship = _uow.Friendships.FirstOrDefault(f => f.UserSenderId == requestUserId && f.UserRecipientId == userId 
                    && f.StatusCodeID == 'R', ""); // prihvata zahtev onaj ko je trenutno ulogovan tj on je recipient, i ako je zahtev u statusu Requested
            ValidationHelper.ValidateNotNull(friendship);

            friendship.StatusCodeID = 'A';
            friendship.FriendshipDate = DateTime.Now;

            _uow.Friendships.Update(friendship, requestUserId, userId);
            _uow.Save();

            return friendship;
        }

        public void DeclineRequest(long userId, long requestUserId) // kao delete metoda, obrisace friendship
        {
            //provera da li postoji useri i prijateljstvo za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var userFriend = _uow.Users.FirstOrDefault(a => a.Id == requestUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var friendship = _uow.Friendships.FirstOrDefault(f => f.UserSenderId == requestUserId 
            && f.UserRecipientId == userId && f.StatusCodeID == 'R', ""); // prihvata zahtev onaj ko je trenutno ulogovan tj on je recipient
            ValidationHelper.ValidateNotNull(friendship);

            _uow.Friendships.Delete(friendship);
            _uow.Save();
        }

        public List<Friendship> FriendRequests(long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var requests = _uow.Friendships.Find(f => f.UserRecipientId == currentUserId && f.StatusCodeID=='R', "").ToList();

            foreach(var request in requests)
            {
                var sender = _uow.Users.FirstOrDefault(u => u.Id == request.UserSenderId, "");
                request.UserSender = sender;
            }

            return requests;
        }

        public List<Friendship> SentFriendRequests(long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var requests = _uow.Friendships.Find(f => f.UserSenderId == currentUserId && f.StatusCodeID == 'R', "").ToList();

            foreach (var request in requests)
            {
                var recipient = _uow.Users.FirstOrDefault(u => u.Id == request.UserRecipientId, "");
                request.UserRecipient = recipient;
            }

            return requests;
        }
        
        public List<User> MutualFriends(long currentUserId, long userId)
        {
            var currUser = _uow.Users.FirstOrDefault(a => a.Id == currentUserId);
            ValidationHelper.ValidateNotNull(currUser);

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            var myFriends = GetAllMyFriends(currentUserId) as List<User>;
            var userFriends = GetAllMyFriends(userId) as List<User>;

            var mutualFriends = myFriends.Intersect(userFriends).ToList();

            return mutualFriends;
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
