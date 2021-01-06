using FakebookNotifications.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsRepo
    {
        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateNotificationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotificationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
