using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;
using System.Security.Claims;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;
using FakebookNotifications.DataAccess.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FakebookNotifications.WebApi.Hubs
{
    [Authorize]
    public class NotificationHub : Hub

    {
        
        public string thisUserEmail = "test@test.com";
        private readonly IUserRepo _userRepo;
        private readonly INotificationsRepo _noteRepo;

        public NotificationHub(IUserRepo userRepo, INotificationsRepo noteRepo)
        {
            _userRepo = userRepo;
            _noteRepo = noteRepo;
        }

        public override async Task OnConnectedAsync()
        {
            if(Context.User?.FindFirst(ClaimTypes.Email) !=null)
            {
                thisUserEmail = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            }          

            if (thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);

                await _userRepo.AddUserConnection(thisUser.Email, Context.ConnectionId);
                if(thisUser.Connections == null)
                {
                    thisUser.Connections = new List<string>();
                }
                thisUser.Connections.Add(Context.ConnectionId);

                List<Domain.Models.Notification> notifications = await _noteRepo.GetAllUnreadNotificationsAsync(thisUserEmail);

                await SendMultipleUserGroupAsync(thisUser, notifications);

            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User?.FindFirst(ClaimTypes.Email) != null)
            {
                thisUserEmail = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            }
            if (thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);
                await _userRepo.RemoveUserConnection(thisUser.Email, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddFollowerAsync(string user, string followed)
        {
            Domain.Models.Notification newNotification = new Domain.Models.Notification
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

        public async Task GetTotalUnreadNotifications(string userEmail)
        {
            List<Domain.Models.Notification> notifications = await _noteRepo.GetAllUnreadNotificationsAsync(userEmail);
            Domain.Models.User thisUser = await _userRepo.GetUserAsync(userEmail);
            await SendMultipleUserGroupAsync(thisUser, notifications);
        }

        public async Task<int> GetUnreadCountAsync(string userEmail)
        {
            int count = await _noteRepo.GetTotalUnreadNotificationsAsync(userEmail);
            return count;
        }

        public async Task CreateNotification(Domain.Models.Notification notification)
        {
            //Create User
            var user = await _userRepo.GetUserAsync(notification.LoggedInUserId);

            //Create notification
            Domain.Models.Notification domainNotification = new Domain.Models.Notification()
            {
                Type = notification.Type,
                LoggedInUserId = notification.LoggedInUserId,
                TriggerUserId = notification.TriggerUserId,
                HasBeenRead = false,
                Date = (DateTime)notification.Date
            };

            //Add to db
            var result = await _noteRepo.CreateNotificationAsync(domainNotification);

            //Check result
            if(result)
            {
                await SendUserGroupAsync(user, domainNotification);
            }
            else
            {
                throw new Exception("Error creating notification");
            }
        }

        public async Task UpdateNotification(Domain.Models.Notification notification)
        {
            //Create notification
            Domain.Models.Notification domainNotification = new Domain.Models.Notification()
            {
                Id = notification.Id,
                Type = notification.Type,
                LoggedInUserId = notification.LoggedInUserId,
                TriggerUserId = notification.TriggerUserId,
                HasBeenRead = notification.HasBeenRead,
                Date = (DateTime)notification.Date
            };

            //update db
            var result = await _noteRepo.UpdateNotificationAsync(domainNotification);

            //Check result
            if (!result)
            {
                throw new Exception("Error creating notification");
            }
        }

        public async Task SendAll(string user, string notification)
        {
            await Clients.All.SendAsync("SendAll", user, notification);
        }

        //Send a notification to all clients within a specific group
        public async Task SendGroup(string group, string notification)
        {
            await Clients.Group(group).SendAsync("SendGroup", notification);
        }

        //Send a notification to the caller
        public async Task SendCaller(Domain.Models.Notification notification)
        {
            var con = this.Context;
            await Clients.Caller.SendAsync("SendCaller", notification);
        }

        //Send a notification to one specific user
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
                    await Clients.Group(user.Email).SendAsync("SendUserGroupAsync", notification);
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

        public async Task SendMultipleUserGroupAsync(Domain.Models.User user, List<Domain.Models.Notification> notifications)
        {
            foreach (string connection in user.Connections)
            {
                await Groups.AddToGroupAsync(connection, user.Email);
            }
            foreach(Domain.Models.Notification note in notifications)
            {
                await Clients.Group(user.Email).SendAsync("SendUserGroupAsync", note);
            }

        }
  
    }
}
 