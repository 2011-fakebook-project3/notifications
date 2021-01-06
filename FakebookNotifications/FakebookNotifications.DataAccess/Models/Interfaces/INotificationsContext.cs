using FakebookNotifications.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace FakebookNotifications.Domain.Interfaces
{
    public interface INotificationsContext
    {
        IMongoCollection<User> User { get; }
        IMongoCollection<Notification> Notifications { get; }
    }
}
