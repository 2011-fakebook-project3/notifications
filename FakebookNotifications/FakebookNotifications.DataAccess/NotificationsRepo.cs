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

        public NotificationsRepo(NotificationsContext context)
        {
            //create context object to work with
            _context = context;

            //Try to connect
            _context.Connect();
        }
    }
}
