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
    }
}
