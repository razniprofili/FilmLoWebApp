using AutoMapper;
using Core;
using Core.Services;
using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : BaseController
    {
        #region Constructors

        public NotificationController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker, 
            INotificationManager notificationManager) : base(mapper, service, checker, notificationManager)
        {
        }

        #endregion

        #region Routes

        [TokenAuthorize]
        [HttpGet(Name = "GetMyNotifications")]
        public List<NotificationModel> GetMyNotifications()
        {
            return facadeNotification.GetAllMyNotifications(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPost("sendNotification")]
        public NotificationModel AddSavedMovie([FromBody] SendNotificationModel notificationModel)
        {
            return facadeNotification.SendNotification(notificationModel, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPut("delete/{id}", Name = "DeleteNotification")]
        public void DeleteNotification(long id)
        {
            facadeNotification.DeleteNotification(CurrentUser.Id, id);
        }

        #endregion

    }
}
