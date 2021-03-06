﻿using System;
using System.Collections.Generic;

namespace FakebookNotifications.Domain.Models
{
    public class User
    {
        public User()
        {
            Connections = new List<string>();
        }
        private string _id;

        /// <summary>
        /// Assigns _id an Id number, after verifying it.
        /// </summary>
        public string Id { get; set; }

        public string Email { get; set; }

        //Contain connection ids of the user for signalr
        public List<string> Connections { get; set; }

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