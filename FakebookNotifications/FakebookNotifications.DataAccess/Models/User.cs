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
        public string Email { get; set; }

        //users connectionid list for signalr
        [BsonElement("connections")]
        public List<string> Connections { get; set; }

        //list of emails that are subscribed to the user
        [BsonElement("subscribers")]
        public List<string> Subscribers { get; set; }

        //list of emails that the user follows
        [BsonElement("follows")]
        public List<string> Follows { get; set; }

        //collection of notifications belonging to the user
        [BsonElement("notifications")]
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
