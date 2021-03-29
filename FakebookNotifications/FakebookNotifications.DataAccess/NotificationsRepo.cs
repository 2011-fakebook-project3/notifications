using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;

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

        public async Task<List<Domain.Models.Notification>> GetAllNotificationsAsync()
        {
            // Get all the notifications from the database
            IAsyncCursor<Notification> all = await _dbCollection.FindAsync(_ => true);
            var dbList = await all.ToListAsync();
            List<Domain.Models.Notification> domainNotes = new();
            foreach (Notification note in dbList)
            {

                Domain.Models.Notification newNote = new()
                {
                    Type = note.Type,
                    LoggedInUserId = note.LoggedInUserId,
                    TriggerUserId = note.TriggerUserId,
                    HasBeenRead = note.HasBeenRead,
                    Date = (DateTime)note.Date,
                    Id = note.Id
                };
                domainNotes.Add(newNote);
            }
            return domainNotes;
        }

        public async Task<bool> CreateNotificationAsync(Domain.Models.Notification notification)
        {
            try
            {
                // Create a new notification based on the passed in Notification.
                Notification newnotification = new()
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

        public async Task<bool> UpdateNotificationAsync(Domain.Models.Notification notification)
        {
            Notification dbNotification = new()
            {
                Id = notification.Id,
                Type = notification.Type,
                LoggedInUserId = notification.LoggedInUserId,
                TriggerUserId = notification.TriggerUserId,
                HasBeenRead = notification.HasBeenRead,
                Date = notification.Date
            };

            try
            {
                // Remove the notification from notifications.
                await _dbCollection.ReplaceOneAsync(x => x.Id == dbNotification.Id, dbNotification);
                // Return true if it suceeds.
                return true;
            }
            catch
            {
                // Return false if it fails.
                return false;
            }
        }

        public async Task<List<Domain.Models.Notification>> GetAllUnreadNotificationsAsync(string userEmail)
        {
            IAsyncCursor<Notification> notifications = await _dbCollection.FindAsync(x => x.LoggedInUserId == userEmail && x.HasBeenRead == false);
            List<Notification> dbNotes = new();
            dbNotes = await notifications.ToListAsync();
            List<Domain.Models.Notification> domainNotes = new();
            foreach (Notification note in dbNotes)
            {

                Domain.Models.Notification newNote = new()
                {
                    Type = note.Type,
                    LoggedInUserId = note.LoggedInUserId,
                    TriggerUserId = note.TriggerUserId,
                    HasBeenRead = note.HasBeenRead,
                    Date = (DateTime)note.Date,
                    Id = note.Id
                };
                domainNotes.Add(newNote);
            }
            return domainNotes;

        }

        public async Task<int> GetTotalUnreadNotificationsAsync(string userEmail)
        {
            IAsyncCursor<Notification> notifications = await _dbCollection.FindAsync(x => x.LoggedInUserId == userEmail && x.HasBeenRead == false);
            return notifications.ToList().Count;
        }

        public async Task<Domain.Models.Notification> GetNotificationAsync(string id)
        {
            IAsyncCursor<Notification> notification = await _dbCollection.FindAsync(x => x.Id == id);
            var dbNote = await notification.FirstAsync();

            Domain.Models.Notification domainNote = new()
            {
                Id = dbNote.Id,
                Type = dbNote.Type,
                LoggedInUserId = dbNote.LoggedInUserId,
                TriggerUserId = dbNote.TriggerUserId,
                HasBeenRead = dbNote.HasBeenRead,
                Date = (DateTime)dbNote.Date
            };

            return domainNote;
        }
    }
}
