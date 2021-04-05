using FakebookNotification.WebApi.Controllers;
using FakebookNotifications.Domain.Interfaces;
using FakebookNotifications.Domain.Models;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakebookNotifications.Testing.IntegrationTests
{
    public class NotificationControllerTest
    {

        private Notification GetDummyNotification(string type)
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

        [Fact]
        public async Task CommentNotificationAsync_CreatesACommentNotificationAsync()
        {
            // arrange
            Mock<IUserRepo> userRepo = new();
            Mock<INotificationsRepo> notificationRepo = new();
            Mock<IHubContext<NotificationHub>> mockHub = new();
            NotificationController controller = new(mockHub.Object, notificationRepo.Object, userRepo.Object);

            Notification dummy = GetDummyNotification("comment");
            int dummyPostId = 0;
            notificationRepo
                .Setup(repo => repo.CreateNotificationAsync(It.IsAny<Notification>()))
                .Verifiable();

            // act
            var result = await controller.CommentNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyPostId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<CreatedAtActionResult>(result);
            notificationRepo.Verify(x => x.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task LikeNotificationAsync_CreatesALikeNotificationAsync()
        {
            // arrange
            Mock<IUserRepo> userRepo = new();
            Mock<INotificationsRepo> notificationRepo = new();
            Mock<IHubContext<NotificationHub>> mockHub = new();
            NotificationController controller = new(mockHub.Object, notificationRepo.Object, userRepo.Object);
            
            Notification dummy = GetDummyNotification("like");
            int dummyPostId = 0;
            notificationRepo
                .Setup(repo => repo.CreateNotificationAsync(It.IsAny<Notification>()))
                .Verifiable();

            // act
            var result = await controller.LikeNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyPostId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<CreatedAtActionResult>(result);
            notificationRepo.Verify(x => x.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task FollowNotificationAsync_CreatesAFollowNotificationAsync()
        {
            // arrange
            Mock<IUserRepo> userRepo = new();
            Mock<INotificationsRepo> notificationRepo = new();
            Mock<IHubContext<NotificationHub>> mockHub = new();
            NotificationController controller = new(mockHub.Object, notificationRepo.Object, userRepo.Object);
            
            Notification dummy = GetDummyNotification("like");
            int dummyPostId = 0;
            notificationRepo
                .Setup(repo => repo.CreateNotificationAsync(It.IsAny<Notification>()))
                .Verifiable();

            // act
            var result = await controller.FollowNotificationAsync(dummy.LoggedInUserId, dummy.TriggerUserId, dummyPostId);

            // assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<CreatedAtActionResult>(result);
            notificationRepo.Verify(x => x.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }
    }
}
