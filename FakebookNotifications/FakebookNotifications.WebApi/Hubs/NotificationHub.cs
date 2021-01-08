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
            
            if(thisUserEmail != "")
            {
                Domain.Models.User thisUser = await _userRepo.GetUserAsync(thisUserEmail);
            }
            // add this connection id to this user in DB

            // get notifcations for user

            // send unread notifications
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // remove this connection from user
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
            await SendUser(followedUser, newNotification);
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
        public async Task SendUser(Domain.Models.User user, Domain.Models.Notification notification)
        {
            throw new NotImplementedException();
            //await Clients.User(user).SendAsync("SendCaller", notification);
           
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
 