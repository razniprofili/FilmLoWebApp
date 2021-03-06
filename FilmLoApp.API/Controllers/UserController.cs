﻿
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
using Common.ResourceParameters;
using System.Text.Json;
//using Domain;
using AutoMapper;
using Core.Services;
using Marvin.Cache.Headers;
using Core;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseController
    {
        #region Constructors

        public UserController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker,
            IUserManager userManager, IFriendshipManager friendshipManager) 
            : base(mapper, service, checker, userManager, friendshipManager)
        {
        }

        #endregion

        #region Routes

        #region Users
        [AllowAnonymous]
        [HttpPost("Register")]
        public object Register([FromBody] RegisterModel registerModel)
        {
            var userModel = facadeUser.Register(registerModel);

            var userToReturn = new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(userModel)
            };

            var links = CreateLinksForUser(userToReturn.AuthResponseData.Id, null);

            var linkedResourceToReturn = userToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetUser",
                new { userId = linkedResourceToReturn["AuthResponseData"] },
                linkedResourceToReturn);

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public object Login([FromBody] LoginModel loginModel)
        {
            var userModel = facadeUser.Login(loginModel);

            var userToReturn = new
            {
                AuthResponseData = SecurityHelper.CreateLoginToken(userModel)
            };

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
            return facadeUser.GetUser(id);
        }

        [TokenAuthorize]
        [HttpGet("allUsers")]
        public List<UserModel> GetAllUsers()
        {
            return facadeUser.GetAllUsers(CurrentUser.Id);
        }

        // pagination, order by, filter... Can be included
        [TokenAuthorize]
        [HttpGet("allUsersWithParameters", Name = "GetUsers")]
        [HttpHead]
        public IActionResult GetAllUsers([FromQuery] ResourceParameters parameters)
        {
            
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            } else
            {
                var usersFromrepo = facadeUser.GetAllUsers(CurrentUser.Id, parameters);

                var paginationMetadata = new
                {
                    totalCount = usersFromrepo.TotalCount,
                    pageSize = usersFromrepo.PageSize,
                    currentPage = usersFromrepo.CurrentPage,
                    totalPages = usersFromrepo.TotalPages
                    //previousPageLink,
                    //nextPageLink
                };

                //dodajemo u response heder klijentu, mozee biti bilo koji format ne mora json
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                //hateoas
                var links = CreateLinksForUser(parameters, usersFromrepo.HasNext, usersFromrepo.HasPrevious);

                var shapedUsers = Mapper.Mapper.Map(usersFromrepo).ShapeData(parameters.Fields);

                var shapedUsersWithLinks = shapedUsers.Select(user =>
                {
                    var userAsDictionary = user as IDictionary<string, object>;
                    var userLinks = CreateLinksForUser((long)userAsDictionary["Id"], null);
                    userAsDictionary.Add("links", userLinks);
                    return userAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    users = shapedUsersWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }
        }


        [TokenAuthorize]
        [HttpPut(Name = "UpdateUser")]
        public ActionResult<UserModel> UpdateUser([FromBody] UpdateModel user)
        {

            var userToReturn = facadeUser.Update(CurrentUser.Id, user);

            var links = CreateLinksForUser(userToReturn.Id, null);

            var linkedResourceToReturn = userToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetUser",
                new { userId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);

        }

        [TokenAuthorize]
        [HttpPut("delete", Name = "DeleteUser")]
        public void DeleteUser()
        {
            facadeUser.Delete(CurrentUser.Id);
        }
        #endregion

        #region Friends
        [TokenAuthorize]
        [HttpGet("friendInfo/{id}")] //friend id
        public FriendshipModel GetFriendInfo(long id)
        {
            return facadeUser.GetFriendInfo(id, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPut("deleteFriend/{id}")] // friend id
        public void DeleteFriend(long id)
        {
            facadeUser.DeleteFriend(id, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("myFriends")]
        public List<UserModel> GetMyFriends()
        {
            return facadeUser.GetAllMyFriends(CurrentUser.Id);

        }

        // pagination, order by, filter... Can be included
        [TokenAuthorize]
        [HttpGet("myFriendsWithParameters", Name = "GetFriends")]
        [HttpHead]
        public IActionResult GetMyFriends([FromQuery] ResourceParameters parameters)
        {
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            }
            else
            {
                var usersFromrepo = facadeUser.GetAllMyFriends(CurrentUser.Id, parameters);

                var paginationMetadata = new
                {
                    totalCount = usersFromrepo.TotalCount,
                    pageSize = usersFromrepo.PageSize,
                    currentPage = usersFromrepo.CurrentPage,
                    totalPages = usersFromrepo.TotalPages
                    //previousPageLink,
                    //nextPageLink
                };

                //dodajemo u response heder klijentu, mozee biti bilo koji format ne mora json
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                //hateoas
                var links = CreateLinksForFriends(parameters, usersFromrepo.HasNext, usersFromrepo.HasPrevious);

                var shapedUsers = Mapper.Mapper.Map(usersFromrepo).ShapeData(parameters.Fields);

                var shapedUsersWithLinks = shapedUsers.Select(user =>
                {
                    var userAsDictionary = user as IDictionary<string, object>;
                    var userLinks = CreateLinksForUser((long)userAsDictionary["Id"], null);
                    userAsDictionary.Add("links", userLinks);
                    return userAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    friends = shapedUsersWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }
             
        }


        [TokenAuthorize]
        [HttpPost("myFriends/search")]
        public List<UserModel> SearchMyFriends([FromBody] string searchCriteria)
        {
            return facadeUser.SearchMyFriends(CurrentUser.Id, searchCriteria);

        }

        [TokenAuthorize]
        [HttpPost("addFriend")]
        public FriendshipModel AddFriend([FromBody] AddFriendshipModel model)
        {
            return facadeUser.Add(model, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpPost("acceptRequest/{id}")] // friend id
        public FriendshipModel AcceptRequest(long id)
        {
            return facadeUser.AcceptRequest(CurrentUser.Id, id);

        }

        [TokenAuthorize]
        [HttpPost("declineRequest/{id}")] // friend id
        public void DeclineRequest(long id)
        {
            facadeUser.DeclineRequest(CurrentUser.Id, id);
        }

        [TokenAuthorize]
        [HttpGet("friendRequests")]
        public List<FriendshipModel> FriendshipRequests()
        {
            return facadeUser.FriendRequests(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("sentFriendRequests")]
        public List<FriendshipModel> SentFriendshipRequests()
        {
            return facadeUser.SentFriendRequests(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("mutualFriends/{userId}")]
        public List<UserModel> GetMutualFriends(long userId)
        {
            return facadeUser.MutualFriends(CurrentUser.Id, userId);
        }
        #endregion

        #endregion

        #region PrivateMethods

        private IEnumerable<LinkDto> CreateLinksForUser(long userId, string fields)
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
              "UPDATE- You must enter fields for update in the body! ONLY current user can be updated!",
              "PUT"));


            return links;
        }

        private string CreateUsersResourceUri(ResourceParameters usersResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUsers",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          orderBy = usersResourceParameters.OrderBy,
                          pageNumber = usersResourceParameters.PageNumber - 1,
                          pageSize = usersResourceParameters.PageSize,
                          searchQuery = usersResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetUsers",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          orderBy = usersResourceParameters.OrderBy,
                          pageNumber = usersResourceParameters.PageNumber + 1,
                          pageSize = usersResourceParameters.PageSize,
                          searchQuery = usersResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current: //vazi isto sto i za def.
                default:
                    return Url.Link("GetUsers",
                    new
                    {
                        fields = usersResourceParameters.Fields,
                        orderBy = usersResourceParameters.OrderBy,
                        pageNumber = usersResourceParameters.PageNumber,
                        pageSize = usersResourceParameters.PageSize,
                        searchQuery = usersResourceParameters.SearchQuery
                    });
            }
        }

        private string CreateFriendsResourceUri(ResourceParameters usersResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetFriends",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          orderBy = usersResourceParameters.OrderBy,
                          pageNumber = usersResourceParameters.PageNumber - 1,
                          pageSize = usersResourceParameters.PageSize,
                          searchQuery = usersResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetFriends",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          orderBy = usersResourceParameters.OrderBy,
                          pageNumber = usersResourceParameters.PageNumber + 1,
                          pageSize = usersResourceParameters.PageSize,
                          searchQuery = usersResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current: //vazi isto sto i za def.
                default:
                    return Url.Link("GetFriends",
                    new
                    {
                        fields = usersResourceParameters.Fields,
                        orderBy = usersResourceParameters.OrderBy,
                        pageNumber = usersResourceParameters.PageNumber,
                        pageSize = usersResourceParameters.PageSize,
                        searchQuery = usersResourceParameters.SearchQuery
                    });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForUser(ResourceParameters usersResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateUsersResourceUri(
                   usersResourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateUsersResourceUri(
                      usersResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateUsersResourceUri(
                        usersResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForFriends(ResourceParameters usersResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateFriendsResourceUri(
                   usersResourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateFriendsResourceUri(
                      usersResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateFriendsResourceUri(
                        usersResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        #endregion

    }
}
