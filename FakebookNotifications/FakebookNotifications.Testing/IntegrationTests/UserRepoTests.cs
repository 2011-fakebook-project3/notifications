using System.Threading.Tasks;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Repositories;
using FakebookNotifications.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace FakebookNotifications.Testing.IntegrationTests
{
    public class UserRepoTests
    {
        private readonly Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private readonly NotificationsDatabaseSettings settings;
        private readonly NullLogger<NotificationsContext> _logger;

        public UserRepoTests()
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
        public async Task CreateUser_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

            //User to create
            Domain.Models.User user = new("1234", "antonio@gmail.com");

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
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

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
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

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
            Mock<IMongoCollection<User>> mockCollection = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            NotificationsRepo noteRepo = new(context);
            UserRepo repo = new(context, noteRepo);
            Domain.Models.Notification newNote = new()
            {
                LoggedInUserId = "test2@test.com",
                TriggerUserId = "test@test.com",
                HasBeenRead = false
            };
            await noteRepo.CreateNotificationAsync(newNote);


            Domain.Models.User newUser = new()
            {
                Email = "test2@test.com"
            };
            await repo.CreateUserAsync(newUser);


            //User to get notifications for
            Domain.Models.User user = await repo.GetUserAsync("test2@test.com");

            //Act
            var result = await repo.TotalUserNotificationsAsync(user);
            var notesToClean = await noteRepo.GetAllUnreadNotificationsAsync("test2@test.com");

            //Cleanup
            foreach (Domain.Models.Notification note in notesToClean)
            {
                await noteRepo.DeleteNotificationAsync(note);
            }

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task DeleteUser_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

            //User to delete
            Domain.Models.User user = await repo.GetUserAsync("antonio@gmail.com");

            //Act
            var result = await repo.DeleteUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddUserConnection_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

            //User to add connection id to
            Domain.Models.User user = await repo.GetUserAsync("antonio@gmail.com");
            var connectionId = "123456789";

            //Act
            var result = await repo.AddUserConnection(user.Email, connectionId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveUserConnection_RepoTest()
        {
            //Arrange
            //Mock collection
            Mock<IMongoCollection<User>> mockCollection = new();
            Mock<INotificationsRepo> noteRepo = new();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            NotificationsContext context = new(_mockSettings.Object, _logger);

            //Create repo to work with
            UserRepo repo = new(context, noteRepo.Object);

            //User to remove connection id from
            Domain.Models.User user = await repo.GetUserAsync("antonio@gmail.com");
            var connectionId = "123456789";

            //Act
            var result = await repo.RemoveUserConnection(user.Email, connectionId);

            //Assert
            Assert.True(result);
        }
    }
}
