using FakebookNotifications.DataAccess.Models;
using MongoDB.Driver;

namespace FakebookNotifications.Domain.Interfaces
{
    public interface INotificationsContext
    {
        IMongoCollection<User> User { get; }
        IMongoCollection<Notification> Notifications { get; }
    }
}
