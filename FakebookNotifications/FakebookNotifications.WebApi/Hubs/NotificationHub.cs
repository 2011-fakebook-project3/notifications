using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FakebookNotifications.WebApi.HubInterfaces;

namespace FakebookNotifications.WebApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {

        public new IHubCallerClients  Clients { get; set; }
        public async Task SendNotification(string user, string notification)
        { 
            throw new NotImplementedException();
        }
   
    }
}
 