using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Repositories;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FakebookNotifications.Testing.IntegrationTests
{
    public class NotificationHubTest
    {
        // Variables used throughout test, mocked to the appropriate interface

        private readonly NotificationHub hub;

        private readonly Domain.Models.User testUser1 = new()
        {
            Id = "01",
            Connections = new List<string>{
                "00", "01", "02"
            },
            Email = "test@test.com"
        };
        private readonly Domain.Models.User testUser2 = new()
        {
            Id = "02",
            Connections = new List<string>{
                "03", "04", "05",
            },
            Email = "notTest@test.com"
        };
        private readonly Domain.Models.Notification updateTestNote = new()
        {
            Id = "5ffca6ca7cf99f8e5c2fae85",
            Type = new KeyValuePair<string, int>("follow", 25),
            HasBeenRead = false,
            TriggerUserId = "notTest@test.com",
            LoggedInUserId = "test@test.com"
        };
        private readonly Domain.Models.Notification testNote = new()
        {
            Type = new KeyValuePair<string, int>("follow", 13),
            HasBeenRead = false,
            TriggerUserId = "notTest@test.com",
            LoggedInUserId = "test@test.com"
        };

        private readonly List<string> groupIds = new()
        {
            "test@test.com",
            "group1",
            "group2",
            "group3"
        };

        private readonly List<string> clientIds = new() { "00", "01", "02", "03", "04", "05" };

        private readonly Mock<IHubCallerClients> mockClients = new();
        private readonly Mock<IGroupManager> mockGroups = new();
        private readonly Mock<IClientProxy> mockClientProxy = new();
        private readonly Mock<HubCallerContext> mockContext = new();

        private readonly UserRepo _userRepo;
        private readonly NotificationsRepo _noteRepo;
        private readonly NotificationsDatabaseSettings settings;
        private readonly NullLogger<NotificationsContext> _logger;

        public NotificationHubTest()
        {
            _logger = new NullLogger<NotificationsContext>();
            //mocking signalr elements for tests
            mockClients.Setup(client => client.All).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Group(groupIds[0])).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Caller).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.OthersInGroup(It.IsIn<string>(groupIds))).Returns(mockClientProxy.Object);
            mockGroups.Setup(group => group.AddToGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new CancellationToken())).Returns(Task.FromResult(true));
            mockGroups.Setup(group => group.RemoveFromGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new CancellationToken())).Returns(Task.FromResult(true));
            mockContext.Setup(context => context.ConnectionId).Returns("01");

            Mock<IOptions<NotificationsDatabaseSettings>> mockSettings = new();
            settings = new NotificationsDatabaseSettings
            {
                ConnectionString = "mongodb+srv://ryan:1234@fakebook.r8oce.mongodb.net/Notifications?retryWrites=true&w=majority",
                DatabaseName = "Notifications",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
            mockSettings.Setup(s => s.Value).Returns(settings);
            NotificationsContext mockDbContext = new(mockSettings.Object, _logger);

            // mocking Mongo db
            NotificationsRepo noteRepo = new(mockDbContext);
            UserRepo userRepo = new(mockDbContext, noteRepo);
            _userRepo = userRepo;
            _noteRepo = noteRepo;

            // creates hub for testing
            hub = new NotificationHub(userRepo, noteRepo)
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object,
            };
        }

        /// <summary>
        /// Tests notfication hub method to send global notifications
        /// </summary>
        [Fact]
        public async Task SendAllVerify()
        {
            //arrange

            // act
            await hub.SendAll("user", "test");

            // assert
            // checks to see if a message was sent to all clients, once and the content is what is expected
            mockClients.Verify(c => c.All, Times.Once);
            mockClients.Verify(c => c.All.SendCoreAsync("SendAll", It.Is<object[]>(o => o != null && o[0] as string == "user" && o[1] as string == "test"), default),
                Times.Once);
        }

        /// <summary>
        /// Tests notfication hub method to send notifications to all users in a group
        /// </summary>
        [Fact]
        public async Task SendGroupVerify()
        {
            //arrange
            string group = groupIds[0];
            var others = groupIds[1];

            // act
            await hub.SendGroup(group, "test");

            // assert
            // checks to see if a message was sent to all clients within a group, once, and not other groups
            mockClients.Verify(c => c.Group(group), Times.Once);
            mockClients.Verify(c => c.Group(others), Times.Never);
        }

        [Fact]
        public async Task SendUserGroupVerify()
        {
            // Arrange

            // Act
            await hub.SendUserGroupAsync(testUser1, testNote);

            // Assert
            mockClients.Verify(c => c.Group(testUser1.Email), Times.Once);
        }

        /// <summary>
        /// Tests notification hub method to send notification back to the user who called the method
        /// </summary>
        [Fact]
        public async Task SendCallerVerify()
        {
            // arrange
            string caller = hub.Context.ConnectionId;

            // act
            await hub.SendCaller(testNote);

            // assert
            // checks to see if a message was sent to the caller-user, once, and not other users
            mockClients.Verify(c => c.Caller, Times.Once);
        }

        [Fact]
        public async Task OnConnectAsyncVerify()
        {
            // Arrange
            mockContext.Setup(u => u.UserIdentifier).Returns("test@test.com");
            Domain.Models.User thisUser = new()
            {
                Id = "53453455",
                Email = "test@test.com",
                Connections = new List<string>(),
            };
            thisUser.Connections.Add(hub.Context.ConnectionId);

            // Act
            await hub.OnConnectedAsync();
            Domain.Models.User test = new();
            test = await _userRepo.GetUserAsync("test@test.com");

            //Assert
            Assert.NotNull(hub.Context.UserIdentifier);
            Assert.NotNull(test.Connections);
            Assert.Equal(thisUser.Connections[0], hub.Context.ConnectionId);
        }

        [Fact]
        public async Task OnDisconnectVerify()
        {
            //Arrange
            mockContext.Setup(u => u.UserIdentifier).Returns("test@test.com");
            Exception exception = new();

            //Act
            await hub.OnDisconnectedAsync(exception);
            var test = await _userRepo.GetUserAsync("test@test.com");

            //Assert
            Assert.DoesNotContain(hub.Context.ConnectionId, test.Connections);
        }

        [Fact]
        public async Task AddFollowersVerify()
        {
            //Arrange
            string user = "notTest@test.com";
            string followed = "test@test.com";

            //Act
            await hub.AddFollowerAsync(user, followed);

            //Assert
            mockClients.Verify(c => c.Group(followed), Times.Once());
        }

        [Fact]
        public async Task GetUnreadNotificationsVerify()
        {
            //Arrange
            var userEmail = testUser1.Email;

            //Act
            await hub.GetTotalUnreadNotifications(userEmail);
            var notes = await _noteRepo.GetAllUnreadNotificationsAsync(userEmail);
            int count = notes.Count;

            //Assert
            mockClients.Verify(c => c.Group(userEmail), Times.Exactly(count));
        }

        [Fact]
        public async Task GetUnreadCountVerify()
        {
            //Arrange

            //Act
            int count = await hub.GetUnreadCountAsync(testUser1.Email);
            //Assert

            Assert.IsType<int>(count);
        }

        [Fact]
        public async Task CreateNotificationAssertTrue()
        {
            //Arrange
            Domain.Models.Notification testNote = new()
            {
                Type = new KeyValuePair<string, int>("follow", 5),
                LoggedInUserId = testUser1.Email,
                TriggerUserId = testUser2.Email
            };

            //Act
            await hub.CreateNotification(testNote);
            List<Domain.Models.Notification> notes = new();
            notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);

            //Assert
            mockClients.Verify(c => c.Group(testUser1.Email), Times.Once);
        }

        [Fact]
        public async Task UpdateNotificationAssertTrue()
        {
            //Arrange
            await _noteRepo.CreateNotificationAsync(testNote);
            List<Domain.Models.Notification> notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);
            Domain.Models.Notification note = new();

            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].LoggedInUserId == updateTestNote.LoggedInUserId && notes[i].TriggerUserId == updateTestNote.TriggerUserId && notes[i].Type.Key == "follow" && notes[i].Type.Value == 13)
                {
                    note = notes[i];
                }
            }
            updateTestNote.Id = note.Id;

            //Act
            await hub.UpdateNotification(updateTestNote);
            notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);
            Domain.Models.Notification newNote = new();
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Id == note.Id)
                {
                    newNote = notes[i];
                }
            }

            //Assert
            Assert.Equal(25, newNote.Type.Value);
            Assert.Equal(testUser1.Email, newNote.LoggedInUserId);
            Assert.Equal(testUser2.Email, newNote.TriggerUserId);
            Assert.False(newNote.HasBeenRead);

        }

        [Fact]
        public async Task MarkAsReadVerify()
        {
            //Arrange
            Domain.Models.Notification noteToRead = new()
            {
                Type = new KeyValuePair<string, int>("follow", 79),
                LoggedInUserId = testUser1.Email,
                TriggerUserId = testUser2.Email,
                HasBeenRead = false
            };
            await _noteRepo.CreateNotificationAsync(noteToRead);
            List<Domain.Models.Notification> notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);
            List<string> readNotes = new();
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].LoggedInUserId == updateTestNote.LoggedInUserId && notes[i].TriggerUserId == updateTestNote.TriggerUserId && notes[i].Type.Key == "follow" && notes[i].Type.Value == 79)
                {
                    readNotes.Add(notes[i].Id);
                }
            }

            // Act
            await hub.MarkNotificationAsReadAsync(readNotes);
            Domain.Models.Notification testNote = await _noteRepo.GetNotificationAsync(readNotes[0]);

            // Assert
            Assert.True(testNote.HasBeenRead);
        }

    }
}
