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

        public async Task<IEnumerable<Domain.Models.Notification>> GetAllNotificationsAsync()
        {
            var all = await _dbCollection.FindAsync(Builders<Notification>.Filter.Empty);
            return (IEnumerable<Domain.Models.Notification>) await all.ToListAsync();
        }

        public async Task<bool> CreateNotificationAsync(Domain.Models.Notification notification)
        {
            try
            {
                Notification newnotification = new Notification()
                {
                    Id = notification.Id,
                    HasBeenRead = notification.HasBeenRead,
                    TriggerUserId = notification.TriggerUserId,
                    LoggedInUserId = notification.LoggedInUserId,
                    Type = notification.Type,
                    Date = notification.Date
                };
                await _dbCollection.InsertOneAsync(newnotification);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> DeleteNotificationAsync(Domain.Models.Notification notification)
        {
            try
            {
                var id = notification.Id;
                await _dbCollection.DeleteOneAsync(Builders<Notification>.Filter.Eq("Id", id));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
