using FakebookNotifications.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess.Models.Interfaces
{
    public interface INotificationsRepo
    {
        // return a enumarble notifications
        IEnumerable<Notification> GetAllNotifications();
        // add a notification to the databse
        Task<bool> AddNotification();
        // remove a notification to the database
        Task<bool> DeleteNotification();
    }
}
