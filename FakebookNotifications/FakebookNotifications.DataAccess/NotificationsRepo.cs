using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsRepo : INotificationsRepo
    {
        private readonly NotificationsContext _context;

        public NotificationsRepo(NotificationsContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Domain.Models.Notification>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateNotificationAsync(Domain.Models.Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotificationAsync(Domain.Models.Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
