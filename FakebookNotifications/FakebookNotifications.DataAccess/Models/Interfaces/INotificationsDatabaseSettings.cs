namespace FakebookNotifications.DataAccess.Models.Interfaces
{
    public interface INotificationsDatabaseSettings
    {
        //Collections
        string NotificationsCollection { get; set; }
        string UserCollection { get; set; }

        //DB Settings
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
