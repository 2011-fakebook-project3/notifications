using FakebookNotifications.DataAccess;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace FakebookNotifications.Testing
{
    public class MongoDbTests
    {
        //Test if connection to database is successfull
        [Fact]
        public void MongoDbConnectionTest()
        {
            //Arrange
            /*var mockSettings = Mock.Of<IOptions<NotificationsDatabaseSettings>>();
            var mockContext = new Mock<NotificationsContext>(mockSettings.Value);

            //Act
            var result = mockContext.Object.Connect();

            //Assert
            Assert.True(result);*/
        }

        [Fact]
        public void MongoUserCollectionTest()
        {

        }

        [Fact]
        public void MongoNotificationCollectionTest()
        {

        }
    }
}
