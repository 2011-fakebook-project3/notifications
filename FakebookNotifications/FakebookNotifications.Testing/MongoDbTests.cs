using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FakebookNotifications.Testing
{
    public class MongoDbTests
    {
        private readonly Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private readonly NotificationsDatabaseSettings settings;
        private readonly NullLogger<NotificationsContext> _logger;

        //Constructor to initialize mocked components before each test
        public MongoDbTests()
        {
            _logger = new NullLogger<NotificationsContext>();
            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            settings = new NotificationsDatabaseSettings()
            {
                ConnectionString = "mongodb+srv://ryan:1234@fakebook.r8oce.mongodb.net/Notifications?retryWrites=true&w=majority",
                DatabaseName = "Notifications",
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

            //Act
            NotificationsContext context = new(_mockSettings.Object, _logger); //create context and test constructor

            //Assert
            //Test is successfull if no exception
        }

        //Test to see if the notification collection is successfully read from db
        [Fact]
        public void MongoNotificationCollectionTest()
        {
            //Arrange
            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);
            NotificationsContext context = new(_mockSettings.Object, _logger); //Create context

            //Act
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
            NotificationsContext context = new(_mockSettings.Object, _logger); //Create context

            //Act
            var userCollection = context.User; //Get collection from context

            //Assert
            Assert.NotNull(userCollection);
        }
    }
}
