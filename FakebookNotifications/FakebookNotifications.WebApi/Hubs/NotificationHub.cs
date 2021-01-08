using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;
using System.Security.Claims;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;
using FakebookNotifications.DataAccess.Models;
using System.Collections.Generic;

namespace FakebookNotifications.WebApi.Hubs
{

    public class NotificationHub : Hub<INotificationHub>
    {
        
        public new IHubCallerClients  Clients { get; set; }
        public string thisUserEmail = "";
        private readonly IUserRepo _userRepo;
        private readonly INotificationsRepo _noteRepo;

        public NotificationHub(IUserRepo userRepo, INotificationsRepo noteRepo)
        {
            _userRepo = userRepo;
            _noteRepo = noteRepo;
        }

        //Send a notification to all clients

        public override async Task OnConnectedAsync()
        {
            thisUserEmail = Context.User?.FindFirst(ClaimTypes.Email)?.Value;

            if (thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);


                await _userRepo.AddUserConnection(thisUser.Email, Context.ConnectionId);

                List<Domain.Models.Notification> notifications = await _noteRepo.GetAllUnreadNotificationsAsync(thisUserEmail);

                await SendMultipleUserGroupAsync(thisUser, notifications);

            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            thisUserEmail = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
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
            Domain.Models.User followedUser = await _userRepo.AddUserSubscriptionAsync(user, followed);
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

        public async Task SendAll(string user, string notification)
        {
            await Clients.All.SendAsync("SendAll", user, notification);
        }

        //Send a notification to all clients within a specific group
        public async Task SendGroup(string group, string notification)
        {
            throw new NotImplementedException();
            //await Clients.Group(group).SendAsync("SendGroup", notification);
        }

        //Send a notification to the caller
        public async Task SendCaller(string caller, string notification)
        {
            //throw new NotImplementedException();
            await Clients.User(caller).SendAsync("SendCaller", notification);
        }

        //Send a notification to one specific user
        public async Task SendUserGroupAsync(Domain.Models.User user, Domain.Models.Notification notification)
        {
            foreach(string connection in user.Connections)
            {
                await AddToGroupAsync(connection, user.Email);
            }
            await Clients.Group(user.Email).SendAsync("SendUserGroupAsync", notification);
           
        }

        public async Task SendMultipleUserGroupAsync(Domain.Models.User user, List<Domain.Models.Notification> notifications)
        {
            foreach (string connection in user.Connections)
            {
                await AddToGroupAsync(connection, user.Email);
            }
            foreach(Domain.Models.Notification note in notifications)
            {
                await Clients.Group(user.Email).SendAsync("SendUserGroupAsync", note);
            }
        

        }

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }
    }
}
 