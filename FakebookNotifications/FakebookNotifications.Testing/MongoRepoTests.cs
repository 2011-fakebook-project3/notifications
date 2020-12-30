using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models.Interfaces;
using MongoDB.Driver;
using Moq;
using System;
using Xunit;

namespace FakebookNotifications.Testing
{
    public class MongoRepoTests
    {
        [Fact]
        public void MongoGetUserByEmailTest()
        {
            //Arrange
            //var mockRepo = new Mock<INotificationsRepo>();

            //mockRepo.Setup(r => r.GetAllUsers()).Returns(new[] { new User("ryan@gmail.com"), new User("antonio@gmail.com"), new User("jordan@gmail.com"), new User("matt@gmail.com")});

            //Act
            //var controller = new NotificationsController(mockedRepo.Object);

            //Assert
        }

        [Fact]
        public void MongoGetNumberOfUsersTest()
        {

        }

        [Fact]
        public void MongoGetNotificationByUserTest()
        {

        }

        [Fact]
        public void MongoGetNumberOfNotificationTest()
        {

        }
    }
}
