using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FakebookNotifications.DataAccess
{
    public class NotificationsContext : INotificationsContext
    {
        private IMongoDatabase _database = null;
        private readonly NotificationsDatabaseSettings _settings;
        private readonly ILogger<NotificationsContext> _logger;

        public NotificationsContext(IOptions<NotificationsDatabaseSettings> settings, ILogger<NotificationsContext> logger)
        {
            //setup logger
            _logger = logger;

            //assign settings to object to be used is other methods
            _settings = settings.Value;

            //Create client and db objects from settings
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);

            //Seed data if empty
            var userCol = User;
            var noteCol = Notifications;

            ClearNotifications(userCol, noteCol);

            if (noteCol == null)
            {
                SeedData(userCol, noteCol);
            }

        }

        //Method to get the user collection
        public IMongoCollection<User> User
        {
            get
            {
                return _database.GetCollection<User>(_settings.UserCollection);
            }
        }

        //Method to get the notification collection
        public IMongoCollection<Notification> Notifications
        {
            get
            {
                return _database.GetCollection<Notification>(_settings.NotificationsCollection);
            }
        }

        //Clear previous seeded notifications
        public bool ClearNotifications(IMongoCollection<User> userCol, IMongoCollection<Notification> noteCol)
        {
            _logger.LogInformation("Clearing previous seed data");

            try
            {
                List<Notification> davidNotes = noteCol.Find(x => x.LoggedInUserId == "david.barnes@revature.net").ToList();
                List<Notification> testNotes = noteCol.Find(x => x.LoggedInUserId == "testaccount@gmail.com").ToList();

                //remove users
                userCol.DeleteOne(x => x.Email == "david.barnes@revature.net");
                userCol.DeleteOne(x => x.Email == "testaccount@gmail.com");

                //remove the users notes
                foreach(var note in davidNotes)
                {
                    noteCol.DeleteOne(x => x.Id == note.Id);
                }

                foreach (var note in testNotes)
                {
                    noteCol.DeleteOne(x => x.Id == note.Id);
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Unable to clear seed data" + ex);
                return false;
            }
        }

        //Seed some initial data
        public bool SeedData(IMongoCollection<User> userCol, IMongoCollection<Notification> noteCol)
        {
            _logger.LogInformation("Attempting to insert seed data");

            try
            {
                //Create seed users
                User user1 = new User();
                user1.Email = "david.barnes@revature.net";
                User user2 = new User();
                user2.Email = "testaccount@gmail.com";

                //insert seed users
                userCol.InsertOne(user1);
                userCol.InsertOne(user2);

                //Create Notifications
                Notification note1 = new Notification()
                {
                    Type = new KeyValuePair<string, int>("Post", 1),
                    LoggedInUserId = "david.barnes@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = false,
                    Date = DateTime.Now
                };
                Notification note2 = new Notification()
                {
                    Type = new KeyValuePair<string, int>("Follow", 0),
                    LoggedInUserId = "david.barnes@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = true,
                    Date = DateTime.Now
                };
                Notification note3 = new Notification()
                {
                    Type = new KeyValuePair<string, int>("Like", 15),
                    LoggedInUserId = "david.barnes@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = false,
                    Date = DateTime.Now
                };
                Notification note4 = new Notification()
                {
                    Type = new KeyValuePair<string, int>("Post", 4),
                    LoggedInUserId = "testaccount@gmail.com",
                    TriggerUserId = "david.barnes@revature.net",
                    HasBeenRead = false,
                    Date = DateTime.Now
                };

                //Insert Notifications
                noteCol.InsertOne(note1);
                noteCol.InsertOne(note2);
                noteCol.InsertOne(note3);
                noteCol.InsertOne(note4);

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Unable to insert seed data" + ex);
                return false;
            }
        }
    }
}
