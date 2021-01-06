using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace FakebookNotifications.Testing
{
    public class UserRepoTests
    {
        private Mock<NotificationsContext> _context;

        public UserRepoTests()
        {
            var settings = new NotificationsDatabaseSettings();
            var _mockSettings = new Mock<IOptions<NotificationsDatabaseSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(settings);

            _context = new Mock<NotificationsContext>(_mockSettings.Object);

            var repo = new UserRepo(_context.Object);
        }

        [Fact]
        public void GetUser_RepoTest()
        {
           
        }

        [Fact]
        public void CreateUser_RepoTest()
        {

        }

        [Fact]
        public void GetUserBySubscription_RepoTest()
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
