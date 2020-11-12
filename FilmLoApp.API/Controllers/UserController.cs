using Domain;
using FilmLoApp.API.Helpers;
using FilmLoApp.API.Models.Friendship;
using FilmLoApp.API.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseController
    {
        [AllowAnonymous] // ne treba nam autorizacija
        [HttpPost("Register")]
        public object Register([FromBody] RegisterModel registerModel)
        {
            var user = UserManager.Register(Mapper.AutoMap<RegisterModel, User>(registerModel));
            // return Mapper.AutoMap<User, UserModel>(user);
            return new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
                //Token = SecurityHelper.CreateLoginToken(user)

            };
        }

        [AllowAnonymous] // ne treba nam autorizacija
        [HttpPost("Login")]
        public object Login([FromBody] LoginModel loginModel)
        {
            var user = UserManager.Login(Mapper.AutoMap<LoginModel, User>(loginModel));
            return new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
                // Token = SecurityHelper.CreateLoginToken(user)
            };
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet]
        public UserModel GetUser(long id)
        {
            var user = UserManager.GetUser(id);
            return Mapper.AutoMap<User, UserModel>(user);
        }

        [TokenAuthorize]
        [HttpGet("allUsers")]
        public List<UserModel> GetAllUsers()
        {
            var users = UserManager.GetAllUsers(CurrentUser.Id);
            return users.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        [TokenAuthorize]
        [HttpPut]
        public UserModel UpdateUser([FromBody] UpdateModel user)
        {

            var newUser = UserManager.Update(CurrentUser.Id, Mapper.AutoMap<UpdateModel, User>(user));
            return Mapper.AutoMap<User, UserModel>(newUser);
        }

        [TokenAuthorize]
        [HttpPut("delete")]
        public void DeleteUser()
        {
            UserManager.DeleteUser(CurrentUser.Id);
        }

        [TokenAuthorize] 
        [HttpGet("friendInfo/{id}")]
        public UserModel GetFriendInfo(long id)
        {
            var user = FriendshipManager.GetFriendInfo(id, CurrentUser.Id);
            return Mapper.AutoMap<User, UserModel>(user);
        }


        [TokenAuthorize] 
        [HttpPut("deleteFriend/{id}")]
        public void DeleteFriend(long id)
        {
            FriendshipManager.DeleteFriend(id, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("myFriends")]
        public List<UserModel> GetMyFriends()
        {
            List<User> friends = FriendshipManager.GetAllMyFriends(CurrentUser.Id);
            return friends.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        [TokenAuthorize]
        [HttpPost("myFriends/search")]
        public List<UserModel> SearchMyFriends([FromBody]string searchCriteria)
        {
            var friends = FriendshipManager.SearchMyFriends(CurrentUser.Id, searchCriteria);
            return friends.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        [TokenAuthorize]
        [HttpPost("addFriend")]
        public FriendshipModel AddFriend( [FromBody] AddFriendshipModel model)
        {
            var friendship = FriendshipManager.Add(Mapper.Map(model, CurrentUser.Id), CurrentUser.Id);
            return Mapper.Map(friendship);
        }

        [TokenAuthorize] 
        [HttpPost("acceptRequest/{id}")]
        public FriendshipModel AcceptRequest(long id)
        {
            var acceptedRequest = FriendshipManager.AcceptRequest(CurrentUser.Id, id);
            return Mapper.Map(acceptedRequest); 
        }

        [TokenAuthorize] 
        [HttpPost("declineRequest/{id}")]
        public void DeclineRequest(long id)
        {
            FriendshipManager.DeclineRequest(CurrentUser.Id, id);
        }
    }
}
