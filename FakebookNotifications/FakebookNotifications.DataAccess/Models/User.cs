using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FakebookNotifications.DataAccess.Models
{

    /// <summary>
    /// A user in the MongoDB collection
    /// </summary>
    public class User
    {

        /// <summary>
        /// Constructor that initializes an empty list of connections
        /// </summary>
        public User()
        {
            Connections = new List<string>();
        }
        
        /// <summary>
        /// Mongo user Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// User's email to identify them
        /// </summary>
        [BsonElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// Users connectionId list for signalr
        /// </summary>
        [BsonElement("connections")]
        public List<string> Connections { get; set; }
    }
}
