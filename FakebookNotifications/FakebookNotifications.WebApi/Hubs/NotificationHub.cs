using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FakebookNotifications.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace FakebookNotifications.WebApi.Hubs
{

    /// <summary>
    /// This class exposes events that other services can listen to/invoke in order to achieve real time notification updates
    /// </summary>
    [Authorize]
    public class NotificationHub : Hub
    {
        private string thisUserEmail = "";
        private readonly IUserRepo _userRepo;
        private readonly INotificationsRepo _noteRepo;

        /// <summary>
        /// Creates a new object by initializing the repositories
        /// </summary>
        /// <param name="userRepo">The User Repo</param>
        /// <param name="noteRepo">The Notification Repo</param>
        public NotificationHub(IUserRepo userRepo, INotificationsRepo noteRepo)
        {
            _userRepo = userRepo;
            _noteRepo = noteRepo;
        }

        /// <summary>
        /// Method that runs on new connections to the hub, grabs user's email from access token, adds connection to that user's db entity,
        /// and sends outstanding unread notifications
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task OnConnectedAsync()
        {
            if (Context.UserIdentifier != null)
            {
                var identity = (ClaimsIdentity)Context.User.Identity;
                thisUserEmail = identity.FindFirst(ClaimTypes.Email).Value;   
            }

            if (thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);

                await _userRepo.AddUserConnection(thisUser.Email, Context.ConnectionId);
                if (thisUser.Connections == null)
                {
                    thisUser.Connections = new List<string>();
                }
                thisUser.Connections.Add(Context.ConnectionId);

                List<Domain.Models.Notification> notifications = await _noteRepo.GetAllUnreadNotificationsAsync(thisUserEmail);

                await SendMultipleUserGroupAsync(thisUser, notifications);

            }
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Removes connection Id from user db entity on disconnect from hub
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.UserIdentifier != null)
            {
                thisUserEmail = Context.UserIdentifier;
            }
            if (thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);
                await _userRepo.RemoveUserConnection(thisUser.Email, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Creates a notification on a new follow and notify the followed user
        /// </summary>
        /// <param name="user"> email of the user following</param>
        /// <param name="followed">email of the user being followerd</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddFollowerAsync(string user, string followed)
        {
            Domain.Models.Notification newNotification = new()
            {
                HasBeenRead = false,
                LoggedInUserId = followed,
                TriggerUserId = user,
                Type = new KeyValuePair<string, int>("follow", 0)
            };
            Domain.Models.User followedUser = await _userRepo.GetUserAsync(followed);
            await _noteRepo.CreateNotificationAsync(newNotification);
            await SendUserGroupAsync(followedUser, newNotification);
        }

        /// <summary>
        /// Method to get all unread notifications from our db for a given user
        /// </summary>
        /// <param name="userEmail">email of the user to be checked for notifications</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task GetTotalUnreadNotifications(string userEmail)
        {
            List<Domain.Models.Notification> notifications = await _noteRepo.GetAllUnreadNotificationsAsync(userEmail);
            Domain.Models.User thisUser = await _userRepo.GetUserAsync(userEmail);
            await SendMultipleUserGroupAsync(thisUser, notifications);
        }

        /// <summary>
        /// method to get the number unread notifications for a given user
        /// </summary>
        /// <param name="userEmail">email of user to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the count of unread notifications.
        /// </returns>
        public async Task<int> GetUnreadCountAsync(string userEmail)
        {
            int count = await _noteRepo.GetTotalUnreadNotificationsAsync(userEmail);
            return count;
        }

        /// <summary>
        /// method to create a new notification for whatever type needed, send notification to user if actively connected
        /// </summary>
        /// <param name="notification">takes in a notification object to add to our db</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CreateNotification(Domain.Models.Notification notification)
        {
            //Create User
            var user = await _userRepo.GetUserAsync(notification.LoggedInUserId);

            //Create notification
            Domain.Models.Notification domainNotification = new()
            {
                Type = notification.Type,
                LoggedInUserId = notification.LoggedInUserId,
                TriggerUserId = notification.TriggerUserId,
                HasBeenRead = false,
                Date = notification.Date
            };

            //Add to db
            var result = await _noteRepo.CreateNotificationAsync(domainNotification);

            //Check result
            if (result)
            {
                await SendUserGroupAsync(user, domainNotification);
            }
            else
            {
                throw new Exception("Error creating notification");
            }
        }

        /// <summary>
        /// updates an exisiting notification for edits
        /// </summary>
        /// <param name="notification">takes in a notification object</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateNotification(Domain.Models.Notification notification)
        {
            //Create notification
            Domain.Models.Notification domainNotification = new()
            {
                Id = notification.Id,
                Type = notification.Type,
                LoggedInUserId = notification.LoggedInUserId,
                TriggerUserId = notification.TriggerUserId,
                HasBeenRead = notification.HasBeenRead,
                Date = notification.Date
            };

            //update db
            var result = await _noteRepo.UpdateNotificationAsync(domainNotification);

            //Check result
            if (!result)
            {
                throw new Exception("Error creating notification");
            }
        }

        /// <summary>
        /// Sends notification to all connected users
        /// </summary>
        /// <param name="user">email of trigger user</param>
        /// <param name="notification">notification to be sent as string</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendAll(string user, string notification)
        {
            await Clients.All.SendAsync("SendAll", user, notification);
        }

        /// <summary>
        /// Send a notification to all clients within a specific group. Can be used to send a notification to
        /// all connected devices of the same user.
        /// </summary>
        /// <param name="group">group name to send notifications to</param>
        /// <param name="notification">notification to be sent as string</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendGroup(string group, string notification)
        {
            await Clients.Group(group).SendAsync("SendGroup", notification);
        }

        /// <summary>
        /// Sends a notification back to the caller
        /// </summary>
        /// <param name="notification">notification to be sent as an object</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendCaller(Domain.Models.Notification notification)
        {
            await Clients.Caller.SendAsync("SendCaller", notification);
        }

        /// <summary>
        /// Send a notification to all connections of one specific user
        /// </summary>
        /// <param name="user">user to send the notification to</param>
        /// <param name="notification">notification to send as an object</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendUserGroupAsync(Domain.Models.User user, Domain.Models.Notification notification)
        {
            try
            {
                if (user != null)
                {
                    foreach (string connection in user.Connections)
                    {
                        await Groups.AddToGroupAsync(connection, user.Email);
                    }
                    await Clients.Group(user.Email).SendAsync("SendUserGroup", notification);
                }
                else
                {
                    Console.WriteLine("User has no connections");
                }
            }
            catch
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Sends notifications to every active connection of a given user
        /// </summary>
        /// <param name="user">user object for the notification to be sent to</param>
        /// <param name="notifications">notification object to be sent</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMultipleUserGroupAsync(Domain.Models.User user, List<Domain.Models.Notification> notifications)
        {
            foreach (string connection in user.Connections)
            {
                await Groups.AddToGroupAsync(connection, user.Email);
            }
            foreach (Domain.Models.Notification note in notifications)
            {
                await Clients.Group(user.Email).SendAsync("SendUserGroupAsync", note);
            }

        }

        /// <summary>
        /// Marks notifications as read, sets all notifications with the given id's HasBeenRead = true;
        /// </summary>
        /// <param name="noteIds">list of string ids of notifications</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkNotificationAsReadAsync(IEnumerable<string> noteIds)
        {
            if (noteIds != null)
            {
                foreach (string id in noteIds)
                {
                    Domain.Models.Notification noteToUpdate = await _noteRepo.GetNotificationAsync(id);
                    noteToUpdate.HasBeenRead = true;
                    await _noteRepo.UpdateNotificationAsync(noteToUpdate);
                }
            }
        }

    }
}
