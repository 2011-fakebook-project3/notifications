using System;

namespace FakebookNotifications.Domain.Models
{
    public class User
    {
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