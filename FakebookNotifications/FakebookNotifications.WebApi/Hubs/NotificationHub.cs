using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FakebookNotifications.WebApi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string notification)
        {
            throw new NotImplementedException();
        }

   
    }
}
