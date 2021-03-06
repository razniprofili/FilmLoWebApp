﻿using Common.Exceptions;
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
   public class UserManager: IUserManager
    {

        #region PrivateFields

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _servicePropertyChecker;
        private readonly IUnitOfWork _uow;

        private readonly IPasswordHelper _passHelper;

        #endregion

        #region Constructors
        public UserManager(IPropertyMappingService propertyMappingService, IPropertyCheckerService checker, IUnitOfWork uow,
            IPasswordHelper passHelper)
        {
            _propertyMappingService = propertyMappingService;
            _servicePropertyChecker = checker;
            _uow = uow;
            _passHelper = passHelper;
        }


        #endregion

        #region Methods

        public User Register(User user)
        {
            var userExists = _uow.Users.Any(a => a.Email == user.Email);
            if (userExists)
                throw new ValidationException("Email currently exists.");

            user.Password = _passHelper.CreateHash(user.Password);
            user.Picture = "https://forum.mikrotik.com/styles/canvas/theme/images/no_avatar.jpg";

            _uow.Users.Add(user, "");
            _uow.Save();

            return user;
        }

        public User Login(User login)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Email.ToLower() == login.Email.Trim().ToLower(), "");

            if (user == null)
                throw new ValidationException("Email not exist.");

            if (!_passHelper.ValidatePassword(login.Password, user.Password))
                throw new ValidationException("Wrong email or password.");

            return user;
        }

        public User GetUser(long id)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == id, "");
            ValidationHelper.ValidateNotNull(user);

            return user;
        }

        public User Update(long id, User user)
        {
            //prvo proveravamo da li postoji user za izmenu, ako postoji nastavljamo dalju izmenu
            var userExists = _uow.Users.GetById(id);
            ValidationHelper.ValidateNotNull(userExists);

            user.Id = id;

            // provere jer korisnik moze da posalje samo jedno polje za promenu, dva, itd..
            if (user.Picture == null)
                user.Picture = userExists.Picture;

            if (user.Name == null)
                user.Name = userExists.Name;

            if (user.Surname == null)
                user.Surname = userExists.Surname;

            if (user.Email == null)
                user.Email = userExists.Email;

            if (user.Password == null)
                user.Password = userExists.Password;


            _uow.Users.Update(user, id);
            _uow.Save();

            return user;
        }

        public void DeleteUser(long id)
        {
            // proveravamo da li postoji user
            var userToDelete = _uow.Users.FirstOrDefault(a => a.Id == id, "");
            ValidationHelper.ValidateNotNull(userToDelete);

            var friends = _uow.Friendships.Find(f => f.UserRecipientId == id || f.UserSenderId == id, "");

            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    _uow.Friendships.Delete(friend);
                }
                _uow.Save();

            }
            _uow.Users.Delete(userToDelete);
            _uow.Save();
        }

        public object GetAllUsers(long idUser, ResourceParameters usersResourceParameters = null)
        {

            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == idUser, "");
            ValidationHelper.ValidateNotNull(user);

            if(usersResourceParameters != null)
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

            var allUsers = _uow.Users.Find(x => x.Id != idUser, ""); //as IQueryable<User> // necemo da nam vraca nas (crr usera)

            if (usersResourceParameters != null)
                return generateResult(allUsers, usersResourceParameters);
            else
                return allUsers.ToList();
               
        }

        #endregion

        #region Private Methods
        private PagedList<User> generateResult(IQueryable<User> allUsers, ResourceParameters usersResourceParameters)
        {
            if (!string.IsNullOrWhiteSpace(usersResourceParameters.SearchQuery))
            {
                var searchQuery = usersResourceParameters.SearchQuery.Trim();
                allUsers = allUsers.Where(a => a.Name.Contains(searchQuery) || a.Surname.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(usersResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var authorPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<UserModel, User>();

                allUsers = allUsers.ApplySort(usersResourceParameters.OrderBy,
                    authorPropertyMappingDictionary);
            }

            return PagedList<User>.Create(allUsers, usersResourceParameters.PageNumber, usersResourceParameters.PageSize);
        }

        #endregion
    }
}
