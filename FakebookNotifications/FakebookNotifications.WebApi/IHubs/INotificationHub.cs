using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakebookNotifications.WebApi.HubInterfaces
{
    public interface INotificationHub    {

        //user and notification params will be replaced with custom objects when available
        
        //Send to all users
         Task SendAll(string user, string notification);

        //Send to Group
        Task SendGroup(string group, string notifications);

        //Send to caller
        Task SendCaller(string user, string notifcations);

        //Send to specific user
        Task SendUser(string user, string notifcations);
    }
}
