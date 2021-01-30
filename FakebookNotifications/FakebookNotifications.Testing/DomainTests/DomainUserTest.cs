using FakebookNotifications.Domain.Models;
using Xunit;

namespace FakebookNotifications.Testing.DomainTests
{
    public class DomainUserTest
    {
        /// <summary>
        /// Tests to see if a new user with correct credentials returns true.
        /// </summary>
        [Fact]
        public void UserIsValidTrue()
        {
            // arrange
            User user = new("3", "jordaneddygarcia@gmail.com");

            // act
            bool valid = user.IsValid(); // should be true

            //assert
            Assert.True(valid);
        }
        /// <summary>
        /// Tests to see if a new user with incorrect credentials returns False.
        /// </summary>
        [Fact]
        public void UserIsValidFalse()
        {
            // arrange
            User user = new("", "Antonio@gmail.com");

            // act
            bool valid = user.IsValid(); // should be false

            //assert
            Assert.False(valid);
        }
    }
}
