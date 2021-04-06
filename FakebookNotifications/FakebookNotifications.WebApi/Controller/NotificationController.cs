
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
        
        /// <summary>
        /// API call from other services to create a comment notification
        /// </summary>
        /// <param>loggedInUser = email of the user who commented</param>
        /// <param>triggerUser = email of user who owns the post</param>
        /// <param>postId = int value of Post ID</param>
        /// <returns>Ok if accepted, Exception if fail</returns>
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
                throw new NullReferenceException(e.ToString());
            }

            return Ok();
        }

        /// <summary>
        /// API call from other services to create a comment notification
        /// </summary>
        /// <param>loggedInUser = email of the user who "Liked" the post</param>
        /// <param>triggerUser = email of user who owns the post</param>
        /// <param>postId = int value of Post ID</param>
        /// <returns>Ok if accepted, Exception if fail</returns>
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

        /// <summary>
        /// API call from other services to create a comment notification
        /// </summary>
        /// <param>loggedInUser = email of the user who followed (follower)</param>
        /// <param>triggerUser = email of user who has been followed (followee)</param>
        /// <param>profileId = int value of follower ID</param>
        /// <returns>Ok if accepted, Exception if fail</returns>
        [HttpPost("follow")]
        public async System.Threading.Tasks.Task<IActionResult> FollowNotificationAsync(string loggedInUser, string triggerUser, int profileId)
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
