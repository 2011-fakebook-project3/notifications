namespace FakebookNotifications.DataAccess.Models.Interfaces
{
    public interface INotificationsDatabaseSettings
    {
        //Collections
        string NotificationsCollection { get; }
        string UserCollection { get; }

        //DB Settings
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
