using System;
using Xunit;
using Moq;
using FakebookNotifications.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using FakebookNotifications.WebApi.HubInterfaces;

namespace FakebookNotifications.Testing

{
    public class NotificationHubTest
    {
        [Fact]
        async public void HubsAreMockableViaDynamic()
        {
            // arrange
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);


            NotificationHub hub = new NotificationHub()
            {
                Clients = mockClients.Object
            };


            // act
            await hub.SendNotification("user", "test");

            // assert
            // checks to see if a message was sent to all clients, once
            mockClients.Verify(c => c.All, Times.Once);

            // checks to see if ...
            mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "SendNotification",
                    It.Is<object[]>(o => o != null && o.Length == 1 && ((object[])o[0]).Length == 3), 
                    default(CancellationToken)),
                Times.Once);


          
        }
    }
}
