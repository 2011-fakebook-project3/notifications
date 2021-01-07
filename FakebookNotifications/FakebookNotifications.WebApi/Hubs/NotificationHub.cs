using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;

namespace FakebookNotifications.WebApi.Hubs
{
<<<<<<< HEAD

    public class NotificationHub : Hub<INotificationHub>
    {
        public new IHubCallerClients  Clients { get; set; }
        public string thisUser = "";

        //Send a notification to all clients

        public override async Task OnConnectedAsync()
        {
            thisUser = Context.UserIdentifier;
            // add this connection id to this user in DB
            // get notifcations for user
            // send unread notifications
            await base.OnConnectedAsync();
        }

        public async Task AddFollower(string user, string followed)
        { 
            string newNotification = "";
            // add followed to user subscription
            // send notification to followed
            await SendUser(followed, newNotification);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // remove this connection from user
            await base.OnDisconnectedAsync(exception);
           

        }




        public async Task SendAll(string user, string notification)
        {
            await Clients.All.SendAsync("SendAll", user, notification);
=======
    public class NotificationHub : Hub<INotificationHub>
    {

        public new IHubCallerClients  Clients { get; set; }

        //Send a notification to all clients
        public async Task SendAll(string user, string notification)
        { 
            throw new NotImplementedException();
>>>>>>> 2adf6a2e660233775a818d0c4531b0c7ecc41756
        }

        //Send a notification to all clients within a specific group
        public async Task SendGroup(string group, string notification)
        {
            throw new NotImplementedException();
<<<<<<< HEAD
            //await Clients.Group(group).SendAsync("SendGroup", notification);
=======
>>>>>>> 2adf6a2e660233775a818d0c4531b0c7ecc41756
        }

        //Send a notification to the caller
        public async Task SendCaller(string caller, string notification)
        {
<<<<<<< HEAD
            //throw new NotImplementedException();
            await Clients.User(caller).SendAsync("SendCaller", notification);
=======
            throw new NotImplementedException();
>>>>>>> 2adf6a2e660233775a818d0c4531b0c7ecc41756
        }

        //Send a notification to one specific user
        public async Task SendUser(string user, string notification)
        {
            throw new NotImplementedException();
<<<<<<< HEAD
            //await Clients.User(user).SendAsync("SendCaller", notification);
           
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
=======
>>>>>>> 2adf6a2e660233775a818d0c4531b0c7ecc41756
        }
    }
}
 