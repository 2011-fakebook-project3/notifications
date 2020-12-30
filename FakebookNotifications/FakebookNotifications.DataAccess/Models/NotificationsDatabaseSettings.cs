using FakebookNotifications.DataAccess.Models.Interfaces;

namespace FakebookNotifications.DataAccess.Models
{
    public class NotificationsDatabaseSettings : INotificationsDatabaseSettings
    {
        //Collections
        public string NotificationsCollection { get; }
        public string UserCollection { get; }

        //DB Settings
        public string ConnectionString { get; }
        public string DatabaseName { get; }
    }
}
