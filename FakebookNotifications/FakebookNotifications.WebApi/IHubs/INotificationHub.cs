using System.Collections.Generic;
using System.Threading.Tasks;
using FakebookNotifications.Domain.Models;

namespace FakebookNotifications.WebApi.HubInterfaces
{
    public interface INotificationHub
    {

        //user and notification params will be replaced with custom objects when available

        //Send to all users
        Task SendAll(User user, Notification notification);

        //Send to Group
        Task SendGroup(string group, Notification notifications);

        //Send to caller
        Task SendCaller(User user, Notification notifcations);

        //Send to specific user
        Task SendUser(User user, Notification notifcations);

        Task SendMultipleUserGroupAsync(User user, List<Notification> notifications);

        Task GetTotalUnreadNotifications(string userEmail);

        Task<int> GetUnreadCountAsync(string userEmail);
    }
}
