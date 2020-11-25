
using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.User;
using Models.Friendship;

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
            var user = facade.Register(registerModel);

            return new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
            };
        }

        [AllowAnonymous] // ne treba nam autorizacija
        [HttpPost("Login")]
        public object Login([FromBody] LoginModel loginModel)
        {
            var user = facade.Login(loginModel);
            return new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
            };
        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet]
        public UserModel GetUser(long id)
        {
            return facade.GetUser(id);
        }

        [TokenAuthorize]
        [HttpGet("allUsers")]
        public List<UserModel> GetAllUsers()
        {
            return facade.GetAllUsers(CurrentUser.Id);  
        }

        [TokenAuthorize]
        [HttpPut]
        public UserModel UpdateUser([FromBody] UpdateModel user)
        {
            return facade.Update(CurrentUser.Id, user);   
        }

        [TokenAuthorize]
        [HttpPut("delete")]
        public void DeleteUser()
        {
            facade.Delete(CurrentUser.Id);
        }

        [TokenAuthorize] 
        [HttpGet("friendInfo/{id}")]
        public UserModel GetFriendInfo(long id)
        {
            return facade.GetFriendInfo(id, CurrentUser.Id);
            
        }

        [TokenAuthorize] 
        [HttpPut("deleteFriend/{id}")]
        public void DeleteFriend(long id)
        {
            facade.DeleteFriend(id, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("myFriends")]
        public List<UserModel> GetMyFriends()
        {
            return facade.GetAllMyFriends(CurrentUser.Id);
            
        }

        [TokenAuthorize]
        [HttpPost("myFriends/search")]
        public List<UserModel> SearchMyFriends([FromBody]string searchCriteria)
        {
            return  facade.SearchMyFriends(CurrentUser.Id, searchCriteria);
           
        }

        [TokenAuthorize]
        [HttpPost("addFriend")]
        public FriendshipModel AddFriend( [FromBody] AddFriendshipModel model)
        {
            return facade.Add(model, CurrentUser.Id);    
        }

        [TokenAuthorize] 
        [HttpPost("acceptRequest/{id}")]
        public FriendshipModel AcceptRequest(long id)
        {
            return facade.AcceptRequest(CurrentUser.Id, id);
            
        }

        [TokenAuthorize] 
        [HttpPost("declineRequest/{id}")]
        public void DeclineRequest(long id)
        {
            facade.DeclineRequest(CurrentUser.Id, id);
        }
    }
}
