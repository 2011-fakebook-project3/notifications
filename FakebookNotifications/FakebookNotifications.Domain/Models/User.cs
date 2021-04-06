using System.Collections.Generic;

namespace FakebookNotifications.Domain.Models
{

    /// <summary>
    /// Represents a user of the app that can be connected to the hub
    /// </summary>
    public class User
    {
        
        /// <summary>
        /// Constructor that intializes the connections list to an empty list
        /// </summary>
        public User()
        {
            Connections = new List<string>();
        }

        /// <summary>
        /// Assigns _id an Id number, after verifying it.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contain connection ids of the user for signalr
        /// </summary>
        public List<string> Connections { get; set; }

        /// <summary>
        /// Constructor that sets a value for Id and email
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="email">email of the user</param>
        public User(string id, string email)
        {
            Id = id;
            Email = email;
        }

        /// <summary>
        /// Checks whether a user is a valid object or not.
        /// </summary>
        /// <returns>A boolean.</returns>
        public bool IsValid()
        {
            return !(string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Email));
        }
    }
}
