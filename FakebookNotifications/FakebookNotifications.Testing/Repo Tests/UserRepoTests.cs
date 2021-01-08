using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakebookNotifications.Domain.Interfaces;

namespace FakebookNotifications.Testing
{
    public class UserRepoTests
    {
        public UserRepoTests()
        {
            
        }

        [Fact]
        public async Task GetUser_RepoTestAsync()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //Act
            var result = await repo.GetUserAsync("ryan@gmail.com");

            //Assert
            Assert.Equal("ryan@gmail.com", result.Email);
        }

        [Fact]
        public async Task CreateUser_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //User to create
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.CreateUserAsync(user);

            //Assert
            Assert.True(result);

        }

        [Fact]
        public async Task GetUsersSubscriptionsById_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //Act
            IEnumerable<Domain.Models.User> subList = await repo.GetUsersSubscriptionsByIdAsync("ryan@gmail.com");
            var resultList = subList.ToList();

            //Assert
            Assert.Equal("antonio@gmail.com", resultList.First().Email);
        }

        [Fact]
        public async Task DeleteUser_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //User to delete
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.DeleteUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUser_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //User to update
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.UpdateUserAsync(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TotalUserNotifications_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //User to get notifications for
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            var result = await repo.TotalUserNotificationsAsync(user);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetUserNotifications_RepoTest()
        {
            //Arrange
            //Mock context
            var context = new Mock<INotificationsContext>();
            var noteRepo = new Mock<INotificationsRepo>();

            //Create repo to work with
            var repo = new UserRepo(context.Object, noteRepo.Object);

            //User to get notifications for
            Domain.Models.User user = new Domain.Models.User("1234", "ryan@gmail.com");

            //Act
            IEnumerable<Domain.Models.Notification> notList = await repo.GetUserNotificationsAsync(user);
            var resultList = notList.ToList();

            //Assert
            Assert.Equal("ryan@gmail.com", resultList.First().LoggedInUserId);
            Assert.Equal("antonio@gmail.com", resultList.First().TriggerUserId);
        }
    }
}
