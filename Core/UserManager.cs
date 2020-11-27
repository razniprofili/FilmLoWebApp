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

        //private readonly IUnitOfWork _uow;

        private readonly IPropertyMappingService _propertyMappingService;

        #endregion

        #region Constructors
        public UserManager(IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }

        //public UserManager(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //}

        #endregion

        #region Methods

        public User Register(User user)
        {
            using (var uow = new UnitOfWork())
            {
                var userExists = uow.UserRepository.Any(a => a.Email == user.Email);
                if (userExists)
                    throw new ValidationException("Email currently exists.");

                user.Password = PasswordHelper.CreateHash(user.Password);
                user.Picture = "https://forum.mikrotik.com/styles/canvas/theme/images/no_avatar.jpg";

                uow.UserRepository.Add(user);
                uow.Save();

                return user;
            }
        }

        public User Login(User login)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Email.ToLower() == login.Email.Trim().ToLower());

                if (user == null)
                    throw new ValidationException("Email not exist.");

                if (!PasswordHelper.ValidatePassword(login.Password, user.Password))
                    throw new ValidationException("Wrong email or password.");

                return user;

            }
        }

        public User GetUser(long id)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == id);
                ValidationHelper.ValidateNotNull(user);
                return user;
            }
        }

        public User Update(long id, User user)
        {
            using (var uow = new UnitOfWork())
            {
                //prvo proveravamo da li postoji user za izmenu, ako postoji nastavljamo dalju izmenu
                var userExists = uow.UserRepository.GetById(id);
                ValidationHelper.ValidateNotNull(userExists);

                user.Id = id;
                user.Picture = userExists.Picture;
                if (user.Name == null)
                    user.Name = userExists.Name;

                if (user.Surname == null)
                    user.Surname = userExists.Surname;

                if (user.Email == null)
                    user.Email = userExists.Email;

                if (user.Password == null)
                    user.Password = userExists.Password;


                uow.UserRepository.Update(user, id);
                uow.Save();

                return user;
            }
        }

        public void DeleteUser(long id)
        {
            using (var uow = new UnitOfWork())
            {

                // proveravamo da li postoji user
                var userToDelete = uow.UserRepository.FirstOrDefault(a => a.Id == id);
                ValidationHelper.ValidateNotNull(userToDelete);

                var friends = uow.FriendshipRepository.Find(f => f.UserRecipientId == id || f.UserSenderId == id);

                if (friends != null)
                {
                    foreach (var friend in friends)
                    {
                        uow.FriendshipRepository.Delete(friend);
                        // uow.Save();
                    }

                    uow.Save();
                }

                uow.UserRepository.Delete(userToDelete);
                uow.Save();
            }
        }

        public List<User> GetAllUsers(long idUser)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var allUsers = uow.UserRepository.Find(x => x.Id != idUser).ToList(); // necemo da nam vraca nas (crr usera)

                return allUsers;
            }
        }

        public PagedList<User> GetAllUsers(long idUser, UsersResourceParameters usersResourceParameters)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == idUser);
                ValidationHelper.ValidateNotNull(user);

                var allUsers = uow.UserRepository.Find(x => x.Id != idUser); //as IQueryable<User> // necemo da nam vraca nas (crr usera)

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
        }

        #endregion


    }
}
