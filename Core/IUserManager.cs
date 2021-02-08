using Common.Helpers;
using Common.ResourceParameters;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
   public interface IUserManager
    {
        public User Register(User user);
        public User Login(User login);
        public User GetUser(long id);
        public User Update(long id, User user);
        public void DeleteUser(long id);
        public object GetAllUsers(long idUser, ResourceParameters usersResourceParameters = null);

    }
}
