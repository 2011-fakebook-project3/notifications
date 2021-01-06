using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsRepo : INotificationsRepo
    {
        private readonly INotificationsContext _context;
        private readonly IMongoCollection<Notification> _dbCollection;

        public NotificationsRepo(INotificationsContext context)
        {
            _context = context;
            _dbCollection = _context.Notifications;
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
