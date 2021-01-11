using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Xunit;
using Moq;
using FakebookNotifications.WebApi.Hubs;
using FakebookNotifications.WebApi.HubInterfaces;
using System.Threading.Tasks;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace FakebookNotifications.Testing

{
    public class NotificationHubTest
    {
        // Variables used throughout test, mocked to the appropriate interface

        private NotificationHub hub;

        private Domain.Models.User testUser1 = new Domain.Models.User
        {
            Id = "01",
            Connections = new List<string>{
                "00", "01", "02"
            },
            Email = "test@test.com"
        };
        private Domain.Models.User testUser2 = new Domain.Models.User
        {
            Id = "02",
            Connections = new List<string>{
                "03", "04", "05",
            },
            Email = "notTest@test.com"
        };
        private Domain.Models.Notification updateTestNote = new Domain.Models.Notification
        {
            Id = "5ffca6ca7cf99f8e5c2fae85",
            Type = new KeyValuePair<string, int>("follow", 0),
            HasBeenRead = false,
            TriggerUserId = "notTest@test.com",
            LoggedInUserId = "test@test.com"
        };
        private Domain.Models.Notification testNote = new Domain.Models.Notification
        {
            Type = new KeyValuePair<string, int>("follow", 0),
            HasBeenRead = false,
            TriggerUserId = "notTest@test.com",
            LoggedInUserId = "test@test.com"
        };

        private List<string> groupIds = new List<string>
        {
            "test@test.com","group1", "group2", "group3"
        };
        private List<string> clientIds = new List<string>() { "00", "01", "02", "03", "04", "05"};
        private Mock<IRequest> request = new Mock<IRequest>();
        private Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
        private Mock<IGroupManager> mockGroups = new Mock<IGroupManager>();
        private Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
        private Mock<HubCallerContext> mockContext = new Mock<HubCallerContext>();


        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        //private Mock<IMongoDatabase> _mockDB;
        private readonly UserRepo _userRepo;
        private readonly NotificationsRepo _noteRepo;
        private NotificationsDatabaseSettings settings; 







        public NotificationHubTest()
        {         
        

        //mocking signalr elements for tests   
            mockClients.Setup(client => client.All).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Group(groupIds[0])).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Caller).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.OthersInGroup(It.IsIn<string>(groupIds))).Returns(mockClientProxy.Object);
            mockGroups.Setup(group => group.AddToGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            mockGroups.Setup(group => group.RemoveFromGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            mockContext.Setup(context => context.ConnectionId).Returns("01");
           
          

           
            Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            settings = new NotificationsDatabaseSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "notificationsDb",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
            _mockSettings.Setup(s => s.Value).Returns(settings);
            var mockDbContext = new NotificationsContext(_mockSettings.Object);


            // mocking Mongo db           
            NotificationsRepo noteRepo = new NotificationsRepo(mockDbContext);
            UserRepo userRepo = new UserRepo(mockDbContext, noteRepo);
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
        async public void SendAllVerify()
        {
            //arranged in constructor

            // act
            await hub.SendAll("user", "test");

            // assert
            // checks to see if a message was sent to all clients, once and the content is what is expected
            mockClients.Verify(c => c.All, Times.Once);
            mockClients.Verify(c => c.All.SendCoreAsync("SendAll", It.Is<object[]>(o => o != null && o[0] == "user" && o[1] == "test"), default(CancellationToken)),
                Times.Once);
        }

        /// <summary>
        /// Tests notfication hub method to send notifications to all users in a group
        /// </summary>
        [Fact]
        async public void SendGroupVerify()
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
        async public void SendUserGroupVerify()
        {
            // Arange
            
            // Act
            await hub.SendUserGroupAsync(testUser1, testNote);

            // Assert
            mockClients.Verify(c => c.Group(testUser1.Email), Times.Once);    
        }


        /// <summary>
        /// Tests notification hub method to send notification back to the user who called the method
        /// </summary>
        [Fact]
        async public void SendCallerVerify()
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
        async public void OnConnectAsyncVerify()
        {
            // Arrange
            Domain.Models.User thisUser = new Domain.Models.User
            {
                Id = "53453455",
                Email = "test@test.com",
                Connections = new List<string>(),

            };
            thisUser.Connections.Add(hub.Context.ConnectionId);

            // Act
            await hub.OnConnectedAsync();
            Domain.Models.User test = new Domain.Models.User();
            test = await _userRepo.GetUserAsync("test@test.com");

            //Assert

            Assert.NotNull(test.Connections);            
            Assert.Equal(thisUser.Connections[0], hub.Context.ConnectionId);
        }

        [Fact]
        async public void OnDisconnectVerify()
        {
            //Arrange
            var exception = new Exception();

            //Act
            await hub.OnDisconnectedAsync(exception);
            var test = await _userRepo.GetUserAsync("test@test.com");

            //Assert
            mockClients.Verify(c => c.All, Times.Never);
            Assert.DoesNotContain(hub.Context.ConnectionId, test.Connections);
        }

        [Fact]
        public async void AddFollowersVerify()
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
        public async void GetUnreadNotificationsVerify()
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
        public async void GetUnreadCountVerify()
        {
            //Arrange

            //Act
            int count  = await hub.GetUnreadCountAsync(testUser1.Email);
            //Assert

            Assert.IsType<int>(count);
        }

        [Fact]
        public async void CreateNotificationAssertTrue()
        {
            //Arrange
            Domain.Models.Notification testNote = new Domain.Models.Notification
            {
                Type = new KeyValuePair<string, int>("follow", 5),
                LoggedInUserId = testUser1.Email,
                TriggerUserId = testUser2.Email
            };

            //Act
            await hub.CreateNotification(testNote);
            List<Domain.Models.Notification> notes = new List<Domain.Models.Notification>();
            notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);

            //Assert             
            mockClients.Verify(c => c.Group(testUser1.Email), Times.Once);
        }

        [Fact]
        public async void UpdateNotificationAssertTrue()
        {
            //Arrange
            

            //Act
            await hub.UpdateNotification(updateTestNote);
            List<Domain.Models.Notification> notes = new List<Domain.Models.Notification>();
            notes = await _noteRepo.GetAllUnreadNotificationsAsync(testUser1.Email);

            //Assert     
            Assert.Equal(5, notes[0].Type.Value);
            Assert.Equal(testUser1.Email, notes[0].LoggedInUserId);
            Assert.Equal(testUser2.Email, notes[0].TriggerUserId);
            Assert.False(notes[0].HasBeenRead);

        }

    }
}