using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using MongoDB.Driver;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace FakebookNotifications.Testing
{
    public class MongoDbTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private Mock<IMongoDatabase> _mockDB;
        private Mock<IMongoClient> _mockClient;
        private NotificationsDatabaseSettings settings;

        //Constructor to initialize mocked components before each test
        public MongoDbTests()
        {
            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

            settings = new NotificationsDatabaseSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "NotificationsDb",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
        }

        //Test for context constructor to see if database is connected to successfully
        [Fact]
        public void MongoContextCreationTest()
        {
            //Arrange
            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Setup Mongo Client
            _mockClient.Setup(c => c.GetDatabase(_mockSettings.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act
            var context = new NotificationsContext(_mockSettings.Object); //create context and test constructor

            //Assert
            Assert.NotNull(context);
        }

        //Test to see if the notification collection is successfully read from db
        [Fact]
        public void MongoNotificationCollectionTest()
        {
            //Arrange
            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Setup Mongo Client
            _mockClient.Setup(c => c.GetDatabase(_mockSettings.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act
            var context = new NotificationsContext(_mockSettings.Object); //Create context
            var notificationCollection = context.Notifications; //Get collection from context

            //Assert 
            Assert.NotNull(notificationCollection);
        }

        //Test to see if the user collection is successfully read from db
        [Fact]
        public void MongoUserCollectionTest()
        {
            //Arrange
            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Setup Mongo Client
            _mockClient.Setup(c => c.GetDatabase(_mockSettings.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act
            var context = new NotificationsContext(_mockSettings.Object); //Create context
            var userCollection = context.User; //Get collection from context

            //Assert 
            Assert.NotNull(userCollection);
        }

        //Tests for user model fields
        [Fact]
        public void UserModel_IdTest()
        {
            //Arrange
            var user = new Mock<User>();
            user.Object.Id = "123123213213";

            //Assert
            Assert.Equal("123123213213", user.Object.Id);
        }

        [Fact]
        public void UserModel_EmailTest()
        {
            //Arrange
            var user = new Mock<User>();
            user.Object.Email = "ryan@gmail.com";

            //Assert
            Assert.Equal("ryan@gmail.com", user.Object.Email);
        }

        [Fact]
        public void UserModel_NotificationsTest()
        {
            //Arrange
            var user = new Mock<User>();
            List<Notification> testNotification = new List<Notification>()
            {
                new Notification()
                {
                    Id = "123",
                    Type = new KeyValuePair<string,int>("Post", 1),
                    LoggedInUserId = "ryan@gmail.com",
                    TriggerUserId = "antonio@gmail.com",
                    HasBeenRead = false,
                    Date = DateTime.Now
                }
            };
            user.Object.Notifications = testNotification;

            var testId = testNotification.First();
            //Assert
            Assert.Equal(testId.Id, user.Object.Notifications.First().Id);
        }

        //Tests for notification model fields
        [Fact]
        public void NotificationModel_IdTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.Id = "123123213213";

            //Assert
            Assert.Equal("123123213213", notification.Object.Id);
        }

        [Fact]
        public void NotificationModel_TypeTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.Type = new KeyValuePair<string, int>("Post", 1);

            //Assert
            Assert.Equal("Post", notification.Object.Type.Key);
        }

        [Fact]
        public void NotificationModel_LoggedInUserTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.LoggedInUserId = "ryan@gmail.com";

            //Assert
            Assert.Equal("ryan@gmail.com", notification.Object.LoggedInUserId);
        }

        [Fact]
        public void NotificationModel_TriggerUserTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.TriggerUserId = "antonio@gmail.com";

            //Assert
            Assert.Equal("antonio@gmail.com", notification.Object.TriggerUserId);
        }

        [Fact]
        public void NotificationModel_BeenReadTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.HasBeenRead = false;

            //Assert
            Assert.False(notification.Object.HasBeenRead);
        }

        [Fact]
        public void NotificationModel_DateTest()
        {
            //Arrange
            var notification = new Mock<Notification>();
            notification.Object.Date = DateTime.Now;

            //Assert
            Assert.Equal(DateTime.Now, notification.Object.Date);
        }

        //Tests for DB Settings
        [Fact]
        public void NotificationSettings_NotificationsCollectionTest()
        {
            //Arrange
            var dbSettings = new Mock<NotificationsDatabaseSettings>();
            dbSettings.Object.NotificationsCollection = "Notifications";

            //Assert
            Assert.Equal("Notifications", dbSettings.Object.NotificationsCollection);
        }

        [Fact]
        public void NotificationSettings_UserCollectionTest()
        {
            //Arrange
            var dbSettings = new Mock<NotificationsDatabaseSettings>();
            dbSettings.Object.UserCollection = "User";

            //Assert
            Assert.Equal("User", dbSettings.Object.UserCollection);
        }

        [Fact]
        public void NotificationSettings_ConnectionStringTest()
        {
            //Arrange
            var dbSettings = new Mock<NotificationsDatabaseSettings>();
            dbSettings.Object.ConnectionString = "localhost";

            //Assert
            Assert.Equal("localhost", dbSettings.Object.ConnectionString);
        }

        [Fact]
        public void NotificationSettings_DatabaseNameTest()
        {
            //Arrange
            var dbSettings = new Mock<NotificationsDatabaseSettings>();
            dbSettings.Object.DatabaseName = "NotificationsDb";

            //Assert
            Assert.Equal("NotificationsDb", dbSettings.Object.DatabaseName);
        }
    }
}
