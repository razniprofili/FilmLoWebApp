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
    [ValidateModel] // to je ono sto smo pisali u Helpers folderu
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
        [HttpPut("{id}")]
        public UserModel UpdateUser(long id, [FromBody] UpdateModel user)
        {

            var newUser = UserManager.Update(id, Mapper.AutoMap<UpdateModel, User>(user));
            return Mapper.AutoMap<User, UserModel>(newUser);
        }

        [TokenAuthorize]
        [HttpPut("delete/{id}")]
        public void DeleteUser(long id)
        {
            UserManager.DeleteUser(id);
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("friendInfo/{id}")]
        public UserModel GetFriendInfo(long id)
        {
            var user = FriendshipManager.GetFriendInfo(id, CurrentUser.Id);
            return Mapper.AutoMap<User, UserModel>(user);
        }


        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("deleteFriend/{id}")]
        public void DeleteFriend(long id)
        {
            FriendshipManager.DeleteFriend(id, CurrentUser.Id);
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("myFriends")]
        public List<UserModel> GetMyFriends()
        {
            List<User> friends = FriendshipManager.GetAllMyFriends(CurrentUser.Id);
            return friends.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        //  public List<User> SearchMyFriends(long idUser, string searchCriteria) URADITI

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("addFriend")]
        public FriendshipModel AddFriend( [FromBody] AddFriendshipModel model)
        {
            var friendship = FriendshipManager.Add(Mapper.AutoMap<AddFriendshipModel, Friendship>(model));
            return Mapper.AutoMap<Friendship, FriendshipModel>(friendship);
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("acceptRequest/{id}")]
        public FriendshipModel AcceptRequest(long id)
        {
            var acceptedRequest = FriendshipManager.AcceptRequest(CurrentUser.Id, id);
            return Mapper.AutoMap<Friendship, FriendshipModel>(acceptedRequest); 
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet("declineRequest/{id}")]
        public void DeclineRequest(long id)
        {
            FriendshipManager.DeclineRequest(CurrentUser.Id, id);
        }
    }
}
