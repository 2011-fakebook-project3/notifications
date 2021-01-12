﻿using FakebookNotifications.DataAccess;
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
    public class NotificationRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;
        private NotificationsDatabaseSettings settings;

        public NotificationRepoTests()
        {
            //Get connection string
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<NotificationRepoTests>()
            .Build();

            var con = configuration.GetValue<string>("TestConnectionString");

            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            settings = new NotificationsDatabaseSettings()
            {
                ConnectionString = con,
                DatabaseName = "TestDb",
                UserCollection = "User",
                NotificationsCollection = "Notifications"
            };
        }

        [Fact]
        public async Task GetAllNotifications_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //Act
            var result = await repo.GetAllNotificationsAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //Notification to create
            Domain.Models.Notification notification = new Domain.Models.Notification()
            {
                Id = "12345",
                Type = new KeyValuePair<string, int>("Follow", 12345),
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
        public async Task UpdateNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //Updated Notification
            var notification = await repo.GetAllUnreadNotificationsAsync("ryan@gmail.com");
            Domain.Models.Notification update = notification.Where(x => x.LoggedInUserId == "ryan@gmail.com" && x.Type.Key == "Follow" && x.Type.Value == 12345).First();
            update.HasBeenRead = true;

            //Act
            var result = await repo.UpdateNotificationAsync(update);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNotification_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //notification to delete
            var notification = await repo.GetAllNotificationsAsync();
            Domain.Models.Notification update = notification.Where(x => x.LoggedInUserId == "ryan@gmail.com" && x.Type.Key == "Follow" && x.Type.Value == 12345 && x.HasBeenRead == true).First();

            //Act
            var result = await repo.DeleteNotificationAsync(update);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllUnreadNotificationsAsync_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            ///Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //Act
            var result = await repo.GetAllUnreadNotificationsAsync("ryan@gmail.com");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTotalUnreadNotificationsAsync_RepoTest()
        {
            //Arrange
            //Mock collection
            var mockCollection = new Mock<IMongoCollection<Notification>>();

            //Setup Context Settings
            _mockSettings.Setup(s => s.Value).Returns(settings);

            //Mock context
            var context = new NotificationsContext(_mockSettings.Object);

            //Create repo to work with
            var repo = new NotificationsRepo(context);

            //Act
            var result = await repo.GetTotalUnreadNotificationsAsync("ryan@gmail.com");

            //Assert
            Assert.Equal(5, result);
        }
    }
}
