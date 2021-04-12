using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace FakebookNotifications.Testing.IntegrationTests
{
    public class NotificationRepoTests
    {
        private readonly Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private readonly NotificationsDatabaseSettings settings;
        private readonly NullLogger<NotificationsContext> _logger;

        public NotificationRepoTests()
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

        [Fact]
        public async Task GetAllNotifications_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Act
            var result = await repo.GetAllNotificationsAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Notification to create
            Domain.Models.Notification notification = new()
            {
                Id = "12345",
                Type = new KeyValuePair<string, int>("Follow", 12345),
                LoggedInUserId = "ryan@gmail.com",
                TriggerUserId = "antonio@gmail.com",
                HasBeenRead = false,
                Date = DateTime.Now
            };

            //Act
            var result = await repo.CreateNotificationAsync(notification);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Updated Notification
            var notification = await repo.GetAllUnreadNotificationsAsync("ryan@gmail.com");
            Domain.Models.Notification update = notification.Where(x => x.LoggedInUserId == "ryan@gmail.com" && x.Type.Key == "Follow" && x.Type.Value == 12345).First();
            update.HasBeenRead = true;

            //Act
            var result = await repo.UpdateNotificationAsync(update);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Notification to create
            Domain.Models.Notification notification = new()
            {
                Id = "42069",
                Type = new KeyValuePair<string, int>("Follow", 12345),
                LoggedInUserId = "ryan@gmail.com",
                TriggerUserId = "antonio@gmail.com",
                HasBeenRead = true,
                Date = DateTime.Now
            };

            //Act
            var result = await repo.CreateNotificationAsync(notification);

            //Assert
            Assert.True(result);

            //notification to delete
            var notifications = await repo.GetAllNotificationsAsync();
            Domain.Models.Notification update = notifications.Where(x => x.LoggedInUserId == "ryan@gmail.com" && x.Type.Key == "Follow" && x.Type.Value == 12345 && x.HasBeenRead == true).First();

            //Act
            result = await repo.DeleteNotificationAsync(update);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllUnreadNotificationsAsync_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            ///Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Act
            var result = await repo.GetAllUnreadNotificationsAsync("ryan@gmail.com");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTotalUnreadNotificationsAsync_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);
            Domain.Models.Notification newNote = new()
            {
                LoggedInUserId = "test3@test.com",
                TriggerUserId = "test@test.com",
                HasBeenRead = false
            };

            await repo.CreateNotificationAsync(newNote);

            //Act
            var result = await repo.GetTotalUnreadNotificationsAsync("test3@test.com");
            var notesToClean = await repo.GetAllUnreadNotificationsAsync("test3@test.com");
            //Cleanup
            foreach (Domain.Models.Notification note in notesToClean)
            {
                await repo.DeleteNotificationAsync(note);
            }

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetNotificationAsync_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<Notification>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo repo = new(context);

            //Act
            var result = await repo.GetNotificationAsync("5ffdc4e76c7bf518ce77cc1a");

            //Assert
            Assert.Equal("test@test.com", result.LoggedInUserId);
        }
    }
}
