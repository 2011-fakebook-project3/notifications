using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using MongoDB.Driver;
using System;
using Xunit;

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
    }
}
