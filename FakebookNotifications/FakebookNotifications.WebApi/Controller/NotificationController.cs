
using System;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace FakebookNotification.WebApi.Controllers
{
    [ApiController]
    [Route("api/notification")]

    public class NotificationController : ControllerBase
    {
        private readonly NotificationHub hub;

        public NotificationController(NotificationHub _hub) => hub = _hub;

        [HttpPost("comment")]
        public async System.Threading.Tasks.Task<IActionResult> CommentNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            FakebookNotifications.Domain.Models.Notification notification = new FakebookNotifications.Domain.Models.Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>("comment", postId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };
            
            try
            {
                await hub.CreateNotification(notification);
                return Ok();
            }
            catch(Exception e) 
            {
                //do something better
                Console.WriteLine(e);
                return StatusCode(500);
            }

        }

        [HttpPost("like")]
        public async System.Threading.Tasks.Task<IActionResult> LikeNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            FakebookNotifications.Domain.Models.Notification notification = new FakebookNotifications.Domain.Models.Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>("like", postId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };
            
            try
            {
                await hub.CreateNotification(notification);
                return Ok();
            }
            catch(Exception e) 
            {
                //do something better
                Console.WriteLine(e);
                return StatusCode(500);
            }

        }

        [HttpPost("follow")]
        public async System.Threading.Tasks.Task<IActionResult> FollowNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            FakebookNotifications.Domain.Models.Notification notification = new FakebookNotifications.Domain.Models.Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>("follow", postId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };
            
            try
            {
                await hub.CreateNotification(notification);
                return Ok();
            }
            catch(Exception e) 
            {
                //do something better
                Console.WriteLine(e);
                return StatusCode(500);
            }

        }
    }
}
