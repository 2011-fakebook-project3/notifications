using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.Domains.Models
{
    /// <summary>
    /// User class that the notification class will use to keep
    /// Track of who has what notifications
    /// </summary>
    public class User
    {
        public string Id { get;}
        public string Email { get; }
    }
}
