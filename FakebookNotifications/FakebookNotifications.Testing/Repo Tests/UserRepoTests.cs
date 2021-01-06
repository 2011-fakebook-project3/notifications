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
    public class UserRepoTests
    {
        private Mock<IOptions<NotificationsDatabaseSettings>> _mockSettings;

        public UserRepoTests()
        {
            var settings = new NotificationsDatabaseSettings();
            _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(settings);
        }

        [Fact]
        public async Task GetUser_RepoTestAsync()
        {
            //Arrange
            //Mock context
            var context = new Mock<NotificationsContext>(_mockSettings.Object);

            //Create repo to work with
            var repo = new UserRepo(context.Object);

            //Act
            var result = await repo.GetUserAsync("ryan@gmail.com");

            //Assert
            Assert.Equal("ryan@gmail.com", result.Email);
        }

        [Fact]
        public void CreateUser_RepoTest()
        {

        }

        [Fact]
        public void GetUsersSubscriptionsById_RepoTest()
        {

        }

        [Fact]
        public void DeleteUser_RepoTest()
        {

        }

        [Fact]
        public void UpdateUser_RepoTest()
        {

        }

        [Fact]
        public void TotalUserNotifications_RepoTest()
        {

        }

        [Fact]
        public void GetUserNotifications_RepoTest()
        {

        }
    }
}
