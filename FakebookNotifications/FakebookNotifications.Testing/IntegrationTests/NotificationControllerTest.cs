using FakebookNotification.WebApi.Controllers;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Repositories;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FakebookNotifications.Testing.IntegrationTests
{
    public class NotificationControllerTest
    {

        private Domain.Models.Notification GetDummyNotification(string type)
        {
            return new()
            {
                Id = "0123",
                Type = new(type, 1),
                LoggedInUserId = "john.werner@revature.net",
                TriggerUserId = "john.werner@revature.net",
                HasBeenRead = false,
                Date = new DateTime(2021, 04, 05)
            };
        }

        private readonly Domain.Models.User testUser1 = new()
        {
            Id = "01",
            Connections = new List<string>{
                "00", "01", "02"
            },
            Email = "john.werner@revature.net"
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
        private readonly NotificationHub mockHub;

        private readonly UserRepo _userRepo;
        private readonly NotificationsRepo _noteRepo;
        private readonly NotificationsDatabaseSettings settings;
        private readonly NullLogger<NotificationsContext> _logger;

        public NotificationControllerTest()
        {
            //mocking signalr elements for tests
            mockClients.Setup(client => client.All).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Group(groupIds[0])).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Caller).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.OthersInGroup(It.IsIn<string>(groupIds))).Returns(mockClientProxy.Object);
            mockGroups.Setup(group => group.AddToGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new CancellationToken())).Returns(Task.FromResult(true));
            mockGroups.Setup(group => group.RemoveFromGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new CancellationToken())).Returns(Task.FromResult(true));
            mockContext.Setup(context => context.ConnectionId).Returns("01");
            

            _logger = new NullLogger<NotificationsContext>();
            Mock<IOptions<NotificationsDatabaseSettings>> mockSettings = new();
            settings = new NotificationsDatabaseSettings
            {
                //ConnectionString = "mongodb+srv://ryan:1234@fakebook.r8oce.mongodb.net/Notifications?retryWrites=true&w=majority",
                ConnectionString = "mongodb://localhost:27017",
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
            mockHub = new NotificationHub(userRepo, noteRepo)
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object,
            };
        }

        [Fact]
        public async Task CommentNotificationAsync_CreatesACommentNotificationAsync()
        {
            // arrange
            Mock<IHubContext<NotificationHub>> mHCtx = new();
            mHCtx.Setup(t => t.Clients.All).Returns(mockClientProxy.Object);
            mHCtx.Setup(t => t.Groups).Returns(mockGroups.Object);
            mHCtx.Setup(t => t.Clients.Group(testUser1.Email)).Returns(mockClientProxy.Object);
            NotificationController controller = new(mHCtx.Object, _noteRepo, _userRepo);

            Domain.Models.Notification dummy = GetDummyNotification("comment");
            int dummyPostId = 0;
            await mockHub.OnConnectedAsync();

            Domain.Models.User thisUser = testUser1;

            await _userRepo.AddUserConnection(thisUser.Email, thisUser.Connections[0]);

            // act
            var result = await controller.CommentNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyPostId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<OkResult>(result);
            await _userRepo.RemoveUserConnection(thisUser.Email, thisUser.Connections[0]);
        }

        [Fact]
        public async Task LikeNotificationAsync_CreatesALikeNotificationAsync()
        {
            // arrange
            Mock<IHubContext<NotificationHub>> mHCtx = new();
            mHCtx.Setup(t => t.Clients.All).Returns(mockClientProxy.Object);
            mHCtx.Setup(t => t.Groups).Returns(mockGroups.Object);
            mHCtx.Setup(t => t.Clients.Group(testUser1.Email)).Returns(mockClientProxy.Object);
            NotificationController controller = new(mHCtx.Object, _noteRepo, _userRepo);

            Domain.Models.Notification dummy = GetDummyNotification("like");
            int dummyPostId = 0;
            await mockHub.OnConnectedAsync();

            Domain.Models.User thisUser = testUser1;

            await _userRepo.AddUserConnection(thisUser.Email, thisUser.Connections[0]);

            // act
            var result = await controller.LikeNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyPostId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<OkResult>(result);
            await _userRepo.RemoveUserConnection(thisUser.Email, thisUser.Connections[0]);
        }

        [Fact]
        public async Task FollowNotificationAsync_CreatesAFollowNotificationAsync()
        {
            // arrange
            Mock<IHubContext<NotificationHub>> mHCtx = new();
            mHCtx.Setup(t => t.Clients.All).Returns(mockClientProxy.Object);
            mHCtx.Setup(t => t.Groups).Returns(mockGroups.Object);
            mHCtx.Setup(t => t.Clients.Group(testUser1.Email)).Returns(mockClientProxy.Object);
            NotificationController controller = new(mHCtx.Object, _noteRepo, _userRepo);

            Domain.Models.Notification dummy = GetDummyNotification("follow");
            int dummyProfileId = 0;
            await mockHub.OnConnectedAsync();

            Domain.Models.User thisUser = testUser1;

            await _userRepo.AddUserConnection(thisUser.Email, thisUser.Connections[0]);

            // act
            var result = await controller.FollowNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyProfileId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<OkResult>(result);
            await _userRepo.RemoveUserConnection(thisUser.Email, thisUser.Connections[0]);
        }
    }
}
