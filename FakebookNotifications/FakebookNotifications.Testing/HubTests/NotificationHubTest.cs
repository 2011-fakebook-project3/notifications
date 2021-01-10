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

namespace FakebookNotifications.Testing

{
    public class NotificationHubTest
    {
        // Variables used throughout test, mocked to the appropriate interface

        private NotificationHub hub;

        private Domain.Models.User testUser1 = new Domain.Models.User
        {
            Id = "01",
            Connections = new List<string>(),
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
        private Domain.Models.Notification testNote = new Domain.Models.Notification
        {
            Id = "58GF023D",
            Type = new KeyValuePair<string, int>("follow", 0),
            HasBeenRead = false,
            TriggerUserId = "notTest@test.com",
            LoggedInUserId = "test@test.com"
        };

        private List<string> groupIds = new List<string>
        {
            "test@test.com","group1", "group2", "group3"
        };
        private List<string> clientIds = new List<string>() { "00", "01", "02", "03", "04", "05" };

        private Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
        private Mock<IGroupManager> mockGroups = new Mock<IGroupManager>();
        private Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
        private Mock<HubCallerContext> mockContext = new Mock<HubCallerContext>();


        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private Mock<IMongoDatabase> _mockDB;
        private NotificationsDatabaseSettings settings;
        private UserRepo userRepo;
        private NotificationsRepo noteRepo;




    


        public NotificationHubTest()
        {


            //mocking signalr elements for tests   
            mockClients.Setup(client => client.All).Returns(mockClientProxy.Object);
            mockClients.Setup(client => client.Group(groupIds[0])).Returns(mockClientProxy.Object);


            mockClients.Setup(client => client.OthersInGroup(It.IsIn<string>(groupIds))).Returns(mockClientProxy.Object);
            mockGroups.Setup(group => group.AddToGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            mockGroups.Setup(group => group.RemoveFromGroupAsync(It.IsIn<string>(clientIds), It.IsIn<string>(groupIds), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            mockContext.Setup(context => context.ConnectionId).Returns("1");

            var mockDbContext = new Mock<INotificationsContext>();
            

            // mocking Mongo db
           
            NotificationsRepo noteRepo = new NotificationsRepo(mockDbContext.Object);
            UserRepo userRepo = new UserRepo(mockDbContext.Object, noteRepo);


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

            mockClients.Verify(c => c.Clients(testUser1.Connections), Times.Once);           

        }        
     

        /// <summary>
        /// Tests notification hub method to send notification back to the user who called the method
        /// </summary>
        [Fact]
        async public void SendCallerVerify()
        {
            string caller = hub.Context.ConnectionId;


            // arrange



            // act
            await hub.SendCaller(caller, "test");

            // assert
            // checks to see if a message was sent to the caller-user, once, and not other users
            mockClients.Verify(c => c.User(caller), Times.Once);
            mockClients.Verify(o => o.AllExcept(caller), Times.Never);
        }

        /// <summary>
        /// Tests notification hub method to send a notification to a specific user
        /// </summary>
        [Fact]
        async public void SendUserVerify()
        {
            // arrange
            var user = "0";

            // act
            //await hub.SendUser(user, "test");

            // assert
            // checks to see if a message was sent to one specific user, once, and not other users
            mockClients.Verify(c => c.User(user), Times.Once);
            mockClients.Verify(c => c.AllExcept("user"), Times.Never);
        }

        [Fact]
        async public void OnConnectAsyncVerify()
        {
            // Arrange
            Domain.Models.User thisUser = new Domain.Models.User();
            thisUser.Connections.Add(hub.Context.UserIdentifier);

            // Act
            await hub.SendUserGroupAsync(thisUser, testNote);

            //Assert

            mockClients.Verify(c => c.Client(thisUser.Connections[0]), Times.Once);
            mockClients.Verify(c => c.All.SendCoreAsync("SendUserGroupAsync", It.Is<object[]>(o => o != null && o[0] == thisUser && o[1] == testNote), default(CancellationToken)),
               Times.Once);
            Assert.Equal(thisUser.Connections[0], hub.Context.UserIdentifier);


        }

        [Fact]
        async public void OnDisconnectVerify()
        {
            //Arrange
            var exception = new Exception();

            //Act
            await hub.OnDisconnectedAsync(exception);

            //Assert
            mockClients.Verify(c => c.All, Times.Never);
        }

        [Fact]
        public async void AddFollowersVerify()
        {
            //Arrange
            string user = "testc@test.com";
            string followed = "notTest@test.com";           



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
            int count = 3;
            List<Notification> notes = new List<Notification>();
            for (int i = 0; i < count; i++)
            {
                Notification newNote = new Notification();
                notes.Add(newNote);
            }

            //Act
            await hub.GetTotalUnreadNotifications(userEmail);

            //Assert
            mockClients.Verify(c => c.Group(userEmail), Times.Exactly(count));
        }

        [Fact]
        public async void GetUnreadCountVerify()
        {
            //Arrange

            //Act
            await hub.GetUnreadCountAsync(testUser1.Email);
            //Assert
            mockClients.Verify(c => c.Group(testUser1.Email), Times.Once); 
        }

    }
}