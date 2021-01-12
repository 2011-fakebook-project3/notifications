﻿using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsContext : INotificationsContext
    {
        private IMongoDatabase _database = null;
        private readonly NotificationsDatabaseSettings _settings;

        public NotificationsContext(IOptions<NotificationsDatabaseSettings> settings)
        {
            //assign settings to object to be used is other methods
            _settings = settings.Value;

            //Create client and db objects from settings
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        //Method to get the user collection
        public IMongoCollection<User> User
        {
            get
            {
                return _database.GetCollection<User>(_settings.UserCollection);
            }
        }

        //Method to get the notification collection
        public IMongoCollection<Notification> Notifications
        {
            get
            {
                return _database.GetCollection<Notification>(_settings.NotificationsCollection);
            }
        }
    }
}
