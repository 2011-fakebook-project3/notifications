using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakebookNotifications.WebApi.HubInterfaces
{
    public interface INotificationHub
    {
         Task SendNotifications(string user, string notification);
    }
}
