using System;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;
using System.Threading.Tasks;

namespace FakebookNotification.WebApi.Controllers
{
    [ApiController]
    [Route("api/notification")]

    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationsRepo _notificationsRepo;
        private readonly IUserRepo _userRepo;

        private enum NotificationType
        {
            Comment,
            Like,
            Follow
        }

        public NotificationController(IHubContext<NotificationHub> hub, INotificationsRepo notificationsRepo, IUserRepo userRepo) 
        { 
            _hub = hub;
            _notificationsRepo = notificationsRepo;
            _userRepo = userRepo;
        }

        /// <summary>
        /// API call from other services to create a comment notification
        /// </summary>
        /// <param name="loggedInUser">email of the user who owns the post</param>
        /// <param name="triggerUser">email of user who commented</param>
        /// <param name="postId">Post ID of the post that was commented on</param>
        /// <returns>Ok if accepted, BadRequest if fail</returns>
        [HttpPost("comment")]
        public async Task<IActionResult> CommentNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            return await CreateNotificationAsync(loggedInUser, triggerUser, postId, NotificationType.Comment);
        }

        /// <summary>
        /// API call from other services to create a like notification
        /// </summary>
        /// <param name="loggedInUser">email of user who owns the post</param>
        /// <param name="triggerUser">email of the user who "Liked" the post</param>
        /// <param name="postId">Post ID of the post that was liked</param>
        /// <returns>Ok if accepted, BadRequest if fail</returns>
        [HttpPost("like")]
        public async Task<IActionResult> LikeNotificationAsync(string loggedInUser, string triggerUser, int postId)
        {
            return await CreateNotificationAsync(loggedInUser, triggerUser, postId, NotificationType.Like);
        }

        /// <summary>
        /// API call from other services to create a follow notification. The user that was followed will get a notification.
        /// </summary>
        /// <param name="loggedInUser">email of user who has been followed (followee)</param>
        /// <param name="triggerUser">email of the user who followed (follower)</param>
        /// <param name="profileId">Profile ID of the trigger user</param>
        /// <returns>Ok if accepted, BadRequest if fail</returns>
        [HttpPost("follow")]
        public async Task<IActionResult> FollowNotificationAsync(string loggedInUser, string triggerUser, int profileId)
        {
            return await CreateNotificationAsync(loggedInUser, triggerUser, profileId, NotificationType.Follow);
        }

        /// <summary>
        /// Helper method to create the notification.
        /// </summary>
        /// <param name="loggedInUser">email of user that will receive the notification</param>
        /// <param name="triggerUser">email of user that triggered the notification event</param>
        /// <param name="linkId">Id of the resource to link to with the notification</param>
        /// <param name="notificationType">The type of the notification (comment/follow/like)</param>
        /// <returns></returns>
        private async Task<IActionResult> CreateNotificationAsync(string loggedInUser, string triggerUser, int linkId, NotificationType notificationType)
        {
            if (loggedInUser == null || triggerUser == null)
            {
                return BadRequest("parameter values cannot be null");
            }

            Notification notification = new Notification
            {
                Type = new System.Collections.Generic.KeyValuePair<string, int>(notificationType.ToString().ToLower(), linkId),
                LoggedInUserId = loggedInUser,
                TriggerUserId = triggerUser,
                Date = DateTime.Now
            };

            string methodName = notificationType switch
            {
                NotificationType.Comment => "CommentNotification",
                NotificationType.Like => "LikeNotification",
                NotificationType.Follow => "FollowNotification",
                _ => throw new ArgumentException("notificationType is not valid", nameof(notificationType))
            };

            await _notificationsRepo.CreateNotificationAsync(notification);
            User user = await _userRepo.GetUserAsync(loggedInUser);

            // Here we send notification to all different places the same user is connected from (laptop, mobile, multiple browsers, etc.).
            foreach (var connection in user.Connections)
            {
                await _hub.Groups.AddToGroupAsync(connection, user.Email);
            }

            await _hub.Clients.Group(user.Email).SendAsync(methodName, notification);
            return Ok();
        }
    }
}
