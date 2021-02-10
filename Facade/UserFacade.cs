using Domain;
using Models.User;
using System;
using Mapper;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.ResourceParameters;

namespace Facade
{
    public partial class FilmLoFacade
    {
        public UserModel Register(RegisterModel registerModel)
        {
            var user =  UserManager.Register(Mapper.Mapper.AutoMap<RegisterModel, User>(registerModel));
            return Mapper.Mapper.AutoMap<User, UserModel>(user);

        }

        public UserModel Login(LoginModel loginModel)
        {
            var user = UserManager.Login(Mapper.Mapper.AutoMap<LoginModel, User>(loginModel));
            return Mapper.Mapper.AutoMap<User, UserModel>(user);

        }

        public UserModel GetUser (long userId)
        {
            var user = UserManager.GetUser(userId);
            return Mapper.Mapper.AutoMap<User, UserModel>(user);
        }

        public List<UserModel> GetAllUsers (long currentUserId)
        {
            var users = UserManager.GetAllUsers(currentUserId) as List<User>;
            return users.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        public PagedList<User> GetAllUsers(long currentUserId, ResourceParameters usersResourceParameters)
        {
            return UserManager.GetAllUsers(currentUserId, usersResourceParameters) as PagedList<User>;
        }

        public UserModel Update(long currentUserId, UpdateModel updateUser)
        {
            var newUser = UserManager.Update(currentUserId, Mapper.Mapper.AutoMap<UpdateModel, User>(updateUser));
            return Mapper.Mapper.AutoMap<User, UserModel>(newUser);
        }

        public void Delete(long currentUserId)
        {
            UserManager.DeleteUser(currentUserId);
        }
    }
}
