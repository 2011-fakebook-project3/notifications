using FakebookNotifications.DataAccess.Interfaces;

namespace FakebookNotifications.DataAccess.Models
{

    /// <summary>
    /// Settings for the database
    /// </summary>
    public class NotificationsDatabaseSettings : INotificationsDatabaseSettings
    {
        
        /// <summary>
        /// The Notification Collection in the database
        /// </summary>
        public string NotificationsCollection { get; set; }

        /// <summary>
        /// The User Collection in the database
        /// </summary>
        public string UserCollection { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database name
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
