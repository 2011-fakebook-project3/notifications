using System;
using System.Collections.Generic;
using FakebookNotifications.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FakebookNotifications.DataAccess.Models
{
    public class NotificationsContext : INotificationsContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly NotificationsDatabaseSettings _settings;
        private readonly ILogger<NotificationsContext> _logger;

        public NotificationsContext(IOptions<NotificationsDatabaseSettings> settings, ILogger<NotificationsContext> logger)
        {

            //setup logger
            _logger = logger;
            //assign settings to object to be used is other methods
            _settings = settings.Value;

            //Create client and db objects from settings
            MongoClient client = new(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
            var userCol = User;
            var noteCol = Notifications;

            //ClearNotifications(userCol, noteCol);


            if (userCol.CountDocuments(new BsonDocument()) == 0)
            {
                SeedUsers(userCol);
            };
            //if (noteCol.CountDocuments(new BsonDocument()) > 20)
            //{
            //    ClearNotifications(noteCol);
            //}
            SeedNotes(noteCol);

        }

        //Method to get the user collection
        public IMongoCollection<User> User => _database.GetCollection<User>(_settings.UserCollection);

        //Method to get the notification collection
        public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>(_settings.NotificationsCollection);
        public bool ClearNotifications(IMongoCollection<Notification> noteCol)
        {
            _logger.LogInformation("Clearing previous seed data");

            try
            {
                List<Notification> davidNotes = noteCol.Find(x => x.LoggedInUserId == "john.werner@revature.net").ToList();
                List<Notification> testNotes = noteCol.Find(x => x.LoggedInUserId == "testaccount@gmail.com").ToList();


                //remove the users notes
                foreach (var note in davidNotes)
                {
                    noteCol.DeleteOne(x => x.Id == note.Id);
                }

                foreach (var note in testNotes)
                {
                    noteCol.DeleteOne(x => x.Id == note.Id);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to clear seed data");
                return false;
            }
        }

        //Seed some initial data
        public bool SeedUsers(IMongoCollection<User> userCol)
        {
            _logger.LogInformation("Attempting to insert seed data");

            try
            {
                //Create seed users
                User user1 = new()
                {
                    Email = "john.werner@revature.net"
                };
                User user2 = new()
                {
                    Email = "testaccount@gmail.com"
                };

                //insert seed users
                userCol.InsertOne(user1);
                userCol.InsertOne(user2);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to insert seed data");
                return false;
            }
        }

        public bool SeedNotes(IMongoCollection<Notification> noteCol)
        {
            _logger.LogInformation("Attempting to insert seed data");

            try
            {

                //Create Notifications
                Notification note1 = new()
                {
                    Type = new KeyValuePair<string, int>("post", 1),
                    LoggedInUserId = "john.werner@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = false,
                    Date = DateTime.Now
                };
                Notification note2 = new()
                {
                    Type = new KeyValuePair<string, int>("follow", 0),
                    LoggedInUserId = "john.werner@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = true,
                    Date = DateTime.Now
                };
                Notification note3 = new()
                {
                    Type = new KeyValuePair<string, int>("like", 15),
                    LoggedInUserId = "john.werner@revature.net",
                    TriggerUserId = "testaccount@gmail.com",
                    HasBeenRead = false,
                    Date = DateTime.Now
                };
                Notification note4 = new()
                {
                    Type = new KeyValuePair<string, int>("post", 4),
                    LoggedInUserId = "testaccount@gmail.com",
                    TriggerUserId = "john.werner@revature.net",
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
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to insert seed data");
                return false;
            }
        }
    }
}
