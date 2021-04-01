using FakebookNotifications.DataAccess.Interfaces;

namespace FakebookNotifications.DataAccess.Models
{
    public class NotificationsDatabaseSettings : INotificationsDatabaseSettings
    {
        //Collections
        public string NotificationsCollection { get; set; }
        public string UserCollection { get; set; }

        //DB Settings
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
