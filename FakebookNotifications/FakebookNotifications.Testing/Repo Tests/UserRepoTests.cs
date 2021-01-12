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
using Microsoft.Extensions.Configuration;

namespace FakebookNotifications.Testing
{
    public class UserRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private NotificationsDatabaseSettings settings;

        public UserRepoTests()
        {
            //Get connection string
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<UserRepoTests>()
            .Build();

            //Get from user secrets
            var con = configuration.GetValue<string>("TestConnectionString");

            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            settings = new NotificationsDatabaseSettings()
            {
                ConnectionString = con,
                DatabaseName = "Notifications",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
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
            var repo = new UserRepo(context, noteRepo.Object);

            //User to create
            Domain.Models.User user = new Domain.Models.User("1234", "antonio@gmail.com");

            //Act
            var result = await repo.CreateUserAsync(user);

            //Assert
            Assert.True(result);

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
            var repo = new UserRepo(context, noteRepo.Object);

            //Act
            var result = await repo.GetUserAsync("ryan@gmail.com");

            //Assert
            Assert.Equal("ryan@gmail.com", result.Email);
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
            var repo = new UserRepo(context, noteRepo.Object);

            //User to update
            Domain.Models.User user = await repo.GetUserAsync("antonio@gmail.com");

            //Act
            var result = await repo.UpdateUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TotalUserNotifications_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<User>>();
        
            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var noteRepo = new NotificationsRepo(context);
            var repo = new UserRepo(context, noteRepo);

            //User to get notifications for
            Domain.Models.User user = await repo.GetUserAsync("ryan@gmail.com");

            //Act
            var result = await repo.TotalUserNotificationsAsync(user);

            //Assert
            Assert.Equal(0, result);
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
            var repo = new UserRepo(context, noteRepo.Object);

            //User to delete
            Domain.Models.User user = await repo.GetUserAsync("antonio@gmail.com");

            //Act
            var result = await repo.DeleteUserAsync(user);

            //Assert
            Assert.True(result);
        }
    }
}
