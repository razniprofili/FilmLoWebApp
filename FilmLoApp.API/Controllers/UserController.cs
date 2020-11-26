
using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.User;
using Models.Friendship;
using Models;

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

            var userToReturn = new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
            };

            //return new
            //{
            //    AuthResponseData = SecurityHelper.CreateLoginToken(user)
            //};

            var links = CreateLinksForUser(userToReturn.AuthResponseData.Id, null);

            var linkedResourceToReturn = userToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetUser",
                new { userId = linkedResourceToReturn["AuthResponseData"] },
                linkedResourceToReturn);

        }

        [AllowAnonymous] // ne treba nam autorizacija
        [HttpPost("Login")]
        public object Login([FromBody] LoginModel loginModel)
        {
            var user = facade.Login(loginModel);

            var userToReturn = new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(user)
            };

            //return new
            //{
            //    AuthResponseData = SecurityHelper.CreateLoginToken(user)
            //};

            var links = CreateLinksForUser(userToReturn.AuthResponseData.Id, null);

            var linkedResourceToReturn = userToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetUser",
                new { userId = linkedResourceToReturn["AuthResponseData"] },
                linkedResourceToReturn);

        }

        [TokenAuthorize] //mora da bude ulogovan
        [HttpGet(Name = "GetUser")]
        public UserModel GetUser(long id)
        {
            return facade.GetUser(id);
        }
         
        // ****************************************************************
        // pagination, order by, filter... Can be included
        [TokenAuthorize]
        [HttpGet("allUsers")]
        public List<UserModel> GetAllUsers()
        {
            return facade.GetAllUsers(CurrentUser.Id);  
        }

        // ****************************************************************

        [TokenAuthorize]
        [HttpPut(Name = "UpdateUser")]
        public ActionResult<UserModel> UpdateUser([FromBody] UpdateModel user)
        {
           
           // return facade.Update(CurrentUser.Id, user); 
            var userToReturn = facade.Update(CurrentUser.Id, user);

            var links = CreateLinksForUser(userToReturn.Id, null);

            var linkedResourceToReturn = userToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetUser",
                new { userId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);

        }


        [TokenAuthorize]
        [HttpPut("delete", Name ="DeleteUser")]
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

        // ****************************************************************
        // pagination, order by, filter... Can be included
        [TokenAuthorize]
        [HttpGet("myFriends")]
        public List<UserModel> GetMyFriends()
        {
            return facade.GetAllMyFriends(CurrentUser.Id);
            
        }
        // ****************************************************************

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

        #region PrivateMethods

        private object CreateLinksForUser(long userId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(Url.Link("GetUser", new { userId }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(Url.Link("GetUser", new { userId, fields }),
                  "self",
                  "GET"));
            }

            links.Add(
               new LinkDto(Url.Link("DeleteUser", new { id = "" }),
               "Only current user can be deleted!",
               "PUT"));

            links.Add(
              new LinkDto(Url.Link("UpdateUser", new { id = "" }),
              "UPDATE- You must enter fields for update in the body!",
              "PUT"));


            return links;
        }

        #endregion

    }
}
