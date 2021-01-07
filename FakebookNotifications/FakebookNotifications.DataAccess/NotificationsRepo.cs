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
            // Get all the notifications from the database
            IAsyncCursor<Notification> all = await _dbCollection.FindAsync(Builders<Notification>.Filter.Empty);
            // Return all the notifications as a list, explicitly casted as an IEnumerable
            return (IEnumerable<Domain.Models.Notification>) await all.ToListAsync();
        }

        public async Task<bool> CreateNotificationAsync(Domain.Models.Notification notification)
        {
            try
            {
                // Create a new notification based on the passed in Notification.
                Notification newnotification = new Notification()
                {
                    HasBeenRead = notification.HasBeenRead,
                    TriggerUserId = notification.TriggerUserId,
                    LoggedInUserId = notification.LoggedInUserId,
                    Type = notification.Type,
                    Date = notification.Date
                };
                // Insert the new notification into the database and return true if it suceeds.
                await _dbCollection.InsertOneAsync(newnotification);
                return true;
            }
            catch
            {
                // Returns false if it fails.
                return false;
            }

        }

        public async Task<bool> DeleteNotificationAsync(Domain.Models.Notification notification)
        {
            try
            {
                // Remove the notification from notifications.
                await _dbCollection.DeleteOneAsync(Builders<Notification>.Filter.Eq("Id", notification.Id));
                // Return true if it suceeds.
                return true;
            }
            catch
            {
                // Return false if it fails.
                return false;
            }
        }
    }
}
