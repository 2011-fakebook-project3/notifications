using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsContext
    {
        private IMongoDatabase _database = null;
        private readonly NotificationsDatabaseSettings _settings;

        public NotificationsContext(IOptions<NotificationsDatabaseSettings> settings)
        {

        }

        //Method to get the user collection
        public IMongoCollection<User> User
        {
            get
            {
                return null;
            }
        }

        //Method to get the notification collection
        public IMongoCollection<Notification> Notifications
        {
            get
            {
                return null;
            }
        }
    }
}
