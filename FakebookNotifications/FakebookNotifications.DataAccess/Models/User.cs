using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FakebookNotifications.DataAccess.Models
{
    public class User
    {
        public User()
        {
            Connections = new List<string>();
        }
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
    }
}
