using System;

namespace FakebookNotifications.Domain.Models
{
    public class User
    {
        private string _id;
        /// <summary>
        /// Assigns _id an Id number, after verifying it.
        /// </summary>
        public string Id {
            get
            {
                return _id;
            }
            set
            {
                // checks to see if int is parsable if not we assign it to a value of 0;
                if (Int32.TryParse(value, out _))
                {
                    _id = value;
                }
                else
                {
                    Console.WriteLine($"Int32.TryParse could not parse '{value}' to an int.");
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
            return !(string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Email));
        }
    }
}
