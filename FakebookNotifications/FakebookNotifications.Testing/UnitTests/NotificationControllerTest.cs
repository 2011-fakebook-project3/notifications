using FakebookNotification.WebApi.Controllers;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FakebookNotifications.Testing.UnitTests
{
    public class NotificationControllerTest
    {

        private readonly Domain.Models.User _testUser1 = new()
        {
            Id = "01",
            Connections = new List<string>{
                "00", "02", "03"
            },
            Email = "test@test.com"
        };

        private readonly Mock<IGroupManager> _mockGroups = new();
        private readonly Mock<IClientProxy> _mockClientProxy = new();
        private readonly Mock<IUserRepo> _mockUserRepo = new();
        private readonly Mock<INotificationsRepo> _mockNoteRepo = new();
        private readonly Mock<IHubContext<NotificationHub>> _mockHubContext = new();

        public NotificationControllerTest()
        {
            _mockHubContext.Setup(t => t.Clients.All).Returns(_mockClientProxy.Object);
            _mockHubContext.Setup(t => t.Groups).Returns(_mockGroups.Object).Verifiable();
            _mockHubContext.Setup(t => t.Clients.Group(_testUser1.Email)).Returns(_mockClientProxy.Object);
            _mockNoteRepo.Setup(t => t.CreateNotificationAsync(It.IsAny<Domain.Models.Notification>())).Verifiable();
            _mockUserRepo.Setup(t => t.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_testUser1).Verifiable();
        }

        private static Domain.Models.Notification GetDummyNotification(string type)
        {
            return new()
            {
                Id = "0123",
                Type = new(type, 1),
                LoggedInUserId = "test@test.com",
                TriggerUserId = "test@test.com",
                HasBeenRead = false,
                Date = new DateTime(2021, 04, 05)
            };
        }

        [Fact]
        public async Task CommentNotificationAsync_ReturnsOk()
        {
            // arrange
            var dummy = GetDummyNotification("comment");

            NotificationController controller = new(_mockHubContext.Object, _mockNoteRepo.Object, _mockUserRepo.Object);

            var postId = 0;

            // act
            var result = await controller.CommentNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, postId);

            // assert
            _mockNoteRepo.Verify(c => c.CreateNotificationAsync(It.IsAny<Domain.Models.Notification>()), Times.Once);
            _mockUserRepo.Verify(c => c.GetUserAsync(It.IsAny<string>()), Times.Once);
            _mockGroups.Verify(c => c.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), new CancellationToken()), Times.Exactly(3));
            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Fact]
        public async Task LikeNotificationAsync_ReturnsOk()
        {
            // arrange
            var dummy = GetDummyNotification("comment");

            NotificationController controller = new(_mockHubContext.Object, _mockNoteRepo.Object, _mockUserRepo.Object);

            var postId = 0;

            // act
            var result = await controller.LikeNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, postId);

            // assert
            _mockNoteRepo.Verify(c => c.CreateNotificationAsync(It.IsAny<Domain.Models.Notification>()), Times.Once);
            _mockUserRepo.Verify(c => c.GetUserAsync(It.IsAny<string>()), Times.Once);
            _mockGroups.Verify(c => c.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), new CancellationToken()), Times.Exactly(3));
            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Fact]
        public async Task FollowNotificationAsync_ReturnsOk()
        {
            // arrange
            var dummy = GetDummyNotification("comment");

            NotificationController controller = new(_mockHubContext.Object, _mockNoteRepo.Object, _mockUserRepo.Object);

            var postId = 0;

            // act
            var result = await controller.FollowNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, postId);

            // assert
            _mockNoteRepo.Verify(c => c.CreateNotificationAsync(It.IsAny<Domain.Models.Notification>()), Times.Once);
            _mockUserRepo.Verify(c => c.GetUserAsync(It.IsAny<string>()), Times.Once);
            _mockGroups.Verify(c => c.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), new CancellationToken()), Times.Exactly(3));
            Assert.IsAssignableFrom<OkResult>(result);
        }
    }
}
