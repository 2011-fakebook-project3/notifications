using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FakebookNotifications.DataAccess.Models
{

    /// <summary>
    /// A notification object in the database
    /// </summary>
    public class Notification
    {

        /// <summary>
        /// Notification Id in the database
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The notification type (post, comment, like, follow)
        /// </summary>
        [BsonElement("type")]
        public KeyValuePair<string, int> Type { get; set; } // ("Post", 1), ("Comment", 45) etc

        /// <summary>
        /// Email of the user that will receive the notifications
        /// </summary>
        [BsonElement("user")]
        public string LoggedInUserId { get; set; }

        /// <summary>
        /// Email of the user that triggered a notification.
        /// </summary>
        [BsonElement("trigger")]
        public string TriggerUserId { get; set; }

        /// <summary>
        /// If notification has been read or not
        /// </summary>
        [BsonElement("read")]
        public bool HasBeenRead { get; set; }

        /// <summary>
        /// The date the notification was created
        /// </summary>
        [BsonElement("date")]
        public BsonDateTime Date { get; set; }
    }
}
