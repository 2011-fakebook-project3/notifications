using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
        public string Type { get; set; }

        //user logged in to receive notifications
        [BsonElement("user")]
        public User LoggedInUser { get; set; }

        //user that triggered a notifiaction by commenting on a user post or tagging a user etc.
        [BsonElement("trigger")]
        public User TriggerUser { get; set; }

        //if notification has been read
        [BsonElement("read")]
        public bool HasBeenRead { get; set; }

        //date of notification
        [BsonElement("date")]
        public BsonDateTime Date { get; set; }
    }
}
