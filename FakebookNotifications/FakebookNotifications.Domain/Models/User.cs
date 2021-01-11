using System;
using System.Collections.Generic;

namespace FakebookNotifications.Domain.Models
{
    public class User
    {
        public User()
        {
            Connections = new List<string>();
            Subscribers = new List<string>();
            Follows = new List<string>();
        }
        private string _id;

        /// <summary>
        /// Assigns _id an Id number, after verifying it.
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                // tries to parse the value
                if (Int32.TryParse(value, out _))
                {
                    _id = value;
                }
                else
                {
                    // assigns the value to an empty string
                    _id = "";
                }
            }
        }

        public string Email { get; set; }

        //Contain connection ids of the user for signalr
        public List<string> Connections { get; set; }

        //Contain emails of users that follow the user
        public List<string> Subscribers { get; set; }

        //Contain emails of users that the user follows
        public List<string> Follows { get; set; }

        public User(string id, string email)
        {
            Id = id;
            Email = email;
        }
    


        /// <summary>
        /// Checks whether a user is a valid object or not.
        /// </summary>
        /// <returns>
        /// A boolean.
        /// </returns>
        public bool IsValid()
        {
            return !(String.IsNullOrEmpty(Id) || String.IsNullOrEmpty(Email)); 
        }
    }
}