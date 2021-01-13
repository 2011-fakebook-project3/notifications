using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FakebookNotifications.Testing
{
    public class UserRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private NotificationsDatabaseSettings settings;
        private NullLogger<NotificationsContext> _logger;

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
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object, _logger);

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
            var context = new NotificationsContext(_mockSettings.Object, _logger);

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
            var context = new NotificationsContext(_mockSettings.Object, _logger);

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
            var context = new NotificationsContext(_mockSettings.Object, _logger);

            //Create repo to work with
            var noteRepo = new NotificationsRepo(context);
            var repo = new UserRepo(context, noteRepo);
            var newNote = new Domain.Models.Notification
            {
                LoggedInUserId = "test2@test.com",
                TriggerUserId = "test@test.com",
                HasBeenRead = false
            };
            await noteRepo.CreateNotificationAsync(newNote);


            var newUser = new Domain.Models.User
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
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object, _logger);

            //Create repo to work with
            var repo = new UserRepo(context, noteRepo.Object);

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
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object, _logger);

            //Create repo to work with
            var repo = new UserRepo(context, noteRepo.Object);

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
            var mockCollection = new Mock<IMongoCollection<User>>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object, _logger);

            //Create repo to work with
            var repo = new UserRepo(context, noteRepo.Object);

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
