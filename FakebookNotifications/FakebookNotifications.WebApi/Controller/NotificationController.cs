
using System;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;

namespace FakebookNotification.WebApi.Controllers
{
    [ApiController]
    [Route("api/notification")]

    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> hub;
        private readonly INotificationsRepo notificationsRepo;
        private readonly IUserRepo userRepo;

        public NotificationController(IHubContext<NotificationHub> _hub, INotificationsRepo _notificationsRepo, IUserRepo _userRepo) 
        { 
            hub = _hub;
            notificationsRepo = _notificationsRepo;
            userRepo = _userRepo;
        }

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
                await notificationsRepo.CreateNotificationAsync(notification);
                User user = await userRepo.GetUserAsync(triggerUser);

                foreach (var connection in user.Connections)
                {
                    await hub.Groups.AddToGroupAsync(connection, user.Email);
                }
                
                await hub.Clients.Group(user.Email).SendAsync("CommentNotification", notification);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new NullReferenceException("User not connected");
            }

            return Ok();
        }

        [HttpPost("like")]
        public async System.Threading.Tasks.Task<IActionResult> LikeNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            FakebookNotifications.Domain.Models.Notification notification = new FakebookNotifications.Domain.Models.Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>("post", postId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };

            try
            {
                await notificationsRepo.CreateNotificationAsync(notification);
                User user = await userRepo.GetUserAsync(triggerUser);

                foreach (var connection in user.Connections)
                {
                    await hub.Groups.AddToGroupAsync(connection, user.Email);
                }
                
                await hub.Clients.Group(user.Email).SendAsync("LikeNotification", notification);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new NullReferenceException("User not connected");
            }
            return Ok();
        }

        [HttpPost("follow")]
        public async System.Threading.Tasks.Task<IActionResult> FollowNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            FakebookNotifications.Domain.Models.Notification notification = new FakebookNotifications.Domain.Models.Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>("follow", profileId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };

            try
            {
                await notificationsRepo.CreateNotificationAsync(notification);
                User user = await userRepo.GetUserAsync(triggerUser);

                foreach (var connection in user.Connections)
                {
                    await hub.Groups.AddToGroupAsync(connection, user.Email);
                }
                
                await hub.Clients.Group(user.Email).SendAsync("FollowNotification", notification);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new NullReferenceException("User not connected");
            }

            return Ok();
        }
    }
}
