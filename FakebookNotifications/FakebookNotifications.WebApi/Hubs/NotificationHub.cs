using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;

namespace FakebookNotifications.WebApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {

        public new IHubCallerClients  Clients { get; set; }

        //Send to All
        public async Task SendAll(string user, string notification)
        { 
            throw new NotImplementedException();
        }

        //Send to Group
        public async Task SendGroup(string group, string notification)
        {
            throw new NotImplementedException();
        }

        //Send to Caller
        public async Task SendCaller(string user, string notification)
        {
            throw new NotImplementedException();
        }

        //Send to User
        public async Task SendUser(string user, string notification)
        {
            throw new NotImplementedException();
        }
    }
}
 