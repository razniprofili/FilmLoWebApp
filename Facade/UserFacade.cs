using Domain;
using Models.User;
using System;
using Mapper;
using System.Collections.Generic;
using System.Linq;

namespace Facade
{
    public partial class FilmLoFacade
    {
        public User Register(RegisterModel registerModel)
        {
            return UserManager.Register(Mapper.Mapper.AutoMap<RegisterModel, User>(registerModel));  
        }

        public User Login(LoginModel loginModel)
        {
            return UserManager.Login(Mapper.Mapper.AutoMap<LoginModel, User>(loginModel));
        }

        public UserModel GetUser (long userId)
        {
            var user = UserManager.GetUser(userId);
            return Mapper.Mapper.AutoMap<User, UserModel>(user);
        }

        public List<UserModel> GetAllUsers (long currentUserId)
        {
            var users = UserManager.GetAllUsers(currentUserId);
            return users.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
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
