using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;

namespace FakebookNotifications.Testing
{
    public class UserRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private NotificationsDatabaseSettings settings;

        public UserRepoTests()
        {
            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            settings = new NotificationsDatabaseSettings()
            {
                ConnectionString = "mongodb://test123",
                DatabaseName = "TestDB",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
        }

        [Fact]
        public async Task GetUser_RepoTestAsync()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //Act
            var result = await repo.Object.GetUserAsync("ryan@gmail.com");

            //Assert
            Assert.Equal("ryan@gmail.com", result.Email);
        }

        [Fact]
        public async Task CreateUser_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //User to create
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.Object.CreateUserAsync(user);

            //Assert
            Assert.True(result);

        }

        [Fact]
        public async Task DeleteUser_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //User to delete
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.Object.DeleteUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUser_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //User to update
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.Object.UpdateUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TotalUserNotifications_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //User to get notifications for
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.Object.TotalUserNotificationsAsync(user);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetUserNotifications_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new Mock<IUserRepo>();

            //User to get notifications for
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            IEnumerable<Domain.Models.Notification> notList = await repo.Object.GetUserNotificationsAsync(user);
            var resultList = notList.ToList();

            //Assert
            Assert.Equal("ryan@gmail.com", resultList.First().LoggedInUserId);
            Assert.Equal("antonio@gmail.com", resultList.First().TriggerUserId);
        }
    }
}
