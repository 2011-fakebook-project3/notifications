using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsRepo: INotificationsRepo
    {
        private readonly NotificationsContext _context;

        public NotificationsRepo(IOptions<NotificationsDatabaseSettings> settings)
        {
            //create context from settings
            _context = new NotificationsContext(settings);

        }

        public Task<bool> AddNotification()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotification()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domains.Models.Notification>> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domains.Models.Notification>> GetNotificationsByEmail(Domains.Models.Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
