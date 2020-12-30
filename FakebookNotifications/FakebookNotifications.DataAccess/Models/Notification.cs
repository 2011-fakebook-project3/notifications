using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace FakebookNotifications.DataAccess.Models
{
    public class Notification
    {
        //mongo object id
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //notification type: post/comment/tag  
        [BsonElement("type")]
        public KeyValuePair<string, int> Type { get; set; } // ("Post", 1), ("Comment", 45) etc

        //email of user logged in to receive notifications
        [BsonElement("user")]
        public string LoggedInUserId { get; set; }

        //email user that triggered a notification by commenting on a user post or tagging a user etc.
        [BsonElement("trigger")]
        public string TriggerUserId { get; set; }

        //if notification has been read
        [BsonElement("read")]
        public bool HasBeenRead { get; set; }

        //date of notification
        [BsonElement("date")]
        public BsonDateTime Date { get; set; }
    }
}
