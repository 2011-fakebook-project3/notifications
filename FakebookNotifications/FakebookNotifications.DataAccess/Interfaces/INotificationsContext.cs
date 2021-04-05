using FakebookNotifications.DataAccess.Models;
using MongoDB.Driver;

namespace FakebookNotifications.DataAccess.Interfaces
{
    public interface INotificationsContext
    {
        IMongoCollection<User> User { get; }
        IMongoCollection<Notification> Notifications { get; }
    }
}
