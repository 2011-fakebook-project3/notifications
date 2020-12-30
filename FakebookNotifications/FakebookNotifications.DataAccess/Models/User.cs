using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace FakebookNotifications.DataAccess.Models
{
    public class User
    {
        //mongo object id
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //users email to identify them
        [BsonElement("email")]
        public string Email { get; }

        //total number of unread notifications user has
        [BsonElement("total")]
        public string TotalUnreadNotifications { get; set; }

        //collection of notifications belonging to the user
        [BsonElement("notifications")]
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
