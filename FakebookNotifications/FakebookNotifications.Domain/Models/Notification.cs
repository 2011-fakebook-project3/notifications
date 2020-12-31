using System;
using System.Collections.Generic;

namespace FakebookNotifications.Domain.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public KeyValuePair<string, int> Type { get; set; }
        public string LoggedInUserId { get; set; }
        public string TriggerUserId { get; set; }
        public bool HasBeenRead { get; set; }
        public DateTime Date { get; set; }
    }
}
