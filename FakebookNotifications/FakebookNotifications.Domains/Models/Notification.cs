using System;
using System.Collections.Generic;

namespace FakebookNotifications.Domains.Models
{
    public class Notification
    {
        /// <summary>
        /// Class used for Keeping track of all notification types.
        /// and a bit of information about the User
        /// </summary>
        public string Id { get; set; }
        public KeyValuePair<string, int> Type { get; set; }
        public string LoggedInUserId { get; set; }
        public string TriggerUserId { get; set; }
        public bool HasBeenRead { get; set; }
        public DateTime Date { get; set; }
    }
}