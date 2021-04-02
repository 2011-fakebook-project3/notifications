using System;
using System.Collections.Generic;

namespace FakebookNotifications.Domain.Models
{

    /// <summary>
    /// Represents a notification that gets sent/received from other services
    /// </summary>
    public class Notification
    {

        /// <summary>
        /// Id of the notification
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type of notification it is. The key is a string that states the type (comment, follow, like).
        /// </summary>
        public KeyValuePair<string, int> Type { get; set; }

        /// <summary>
        /// The Email of user that the notification should be sent to
        /// </summary>
        public string LoggedInUserId { get; set; }

        /// <summary>
        /// The Email of user that caused the notification to be sent
        /// </summary>
        public string TriggerUserId { get; set; }

        /// <summary>
        /// Whether the notification has been read by the user or not
        /// </summary>
        public bool HasBeenRead { get; set; }

        /// <summary>
        /// The time when the notification was created
        /// </summary>
        public DateTime Date { get; set; }
    }
}
