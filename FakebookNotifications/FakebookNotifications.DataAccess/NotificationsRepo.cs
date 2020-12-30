using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsRepo: INotificationsRepo
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Notification> _notifications;
        private NotificationsDatabaseSettings _settings;

        public NotificationsRepo(IOptions<NotificationsDatabaseSettings> settings)
        {
            //Get settings
            _settings = settings.Value;

            //Create client and db objects from settings
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);

            //create collection objects to be used in repo methods
            _users = database.GetCollection<User>(_settings.UserCollection);
            _notifications = database.GetCollection<Notification>(_settings.NotificationsCollection);
        }
    }
}
