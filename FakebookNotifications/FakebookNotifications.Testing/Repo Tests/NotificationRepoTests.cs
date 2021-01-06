using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakebookNotifications.Testing
{
    public class NotificationRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;

        public NotificationRepoTests()
        {
            var settings = new NotificationsDatabaseSettings();
            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(settings);
        }

        [Fact]
        public async Task GetAllNotifications_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<NotificationsContext>(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context.Object);

            //Act
            var result = await repo.GetAllNotificationsAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateNotification_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<NotificationsContext>(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context.Object);

            //Notification to create
            Domain.Models.Notification notification = new Domain.Models.Notification()
            {
                Id = "12345",
                Type = new KeyValuePair<string, int>("Follow", 1234),
                LoggedInUserId = "ryan@gmail.com",
                TriggerUserId = "antonio@gmail.com",
                HasBeenRead = false,
                Date = DateTime.Now
            };

            //Act
            var result = await repo.CreateNotificationAsync(notification);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNotification_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<NotificationsContext>(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context.Object);

            //Notification to delete
            Domain.Models.Notification notification = new Domain.Models.Notification()
            {
                Type = new KeyValuePair<string, int>("Follow", 1234),
                LoggedInUserId = "ryan@gmail.com",
                TriggerUserId = "antonio@gmail.com",
                HasBeenRead = false,
                Date = DateTime.Now
            };

            //Act
            var result = await repo.CreateNotificationAsync(notification);

            //Assert
            Assert.True(result);
        }
    }
}
