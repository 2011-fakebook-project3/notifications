using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;

namespace FakebookNotifications.WebApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {

        public new IHubCallerClients  Clients { get; set; }

        //Send a notfication to all clients
        public async Task SendAll(string user, string notification)
        { 
            throw new NotImplementedException();
        }

        //Send a notfication to all clients within a specific group
        public async Task SendGroup(string group, string notification)
        {
            throw new NotImplementedException();
        }

        //Send a notfication to the caller
        public async Task SendCaller(string caller, string notification)
        {
            throw new NotImplementedException();
        }

        //Send a notfication to one specific user
        public async Task SendUser(string user, string notification)
        {
            throw new NotImplementedException();
        }
    }
}
 