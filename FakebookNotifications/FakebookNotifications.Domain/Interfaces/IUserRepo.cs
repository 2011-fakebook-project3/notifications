using FakebookNotifications.Domain.Models;
using System.Threading.Tasks;

namespace FakebookNotifications.Domain.Interfaces
{
    public interface IUserRepo
    {
        /// <summary>
        /// Gets a User by their UserId.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a User object.
        /// </returns>
        Task<User> GetUserAsync(string email);
        /// <summary>
        /// Creates a User
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful Delete.
        /// </returns>
        Task<bool> CreateUserAsync(User user);
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <returns> 
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful delete.
        /// </returns>
        Task<bool> DeleteUserAsync(User user);
        /// <summary>
        /// Updates a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful update.
        /// </returns>
        Task<bool> UpdateUserAsync(User user);
        /// <summary>
        /// Returns the number of total notifications a user has.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an int to represent unread notifications.
        /// </returns>
        Task<int> TotalUserNotificationsAsync(User user);
        /// <summary>
        /// Adds a connection id to the users collection for signalr to send notifications to
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="connectionId">the connectionid to be added to the users collection</param>
        /// <returns>Bool if the update was successfull</returns>
        Task<bool> AddUserConnection(string email, string connectionId);

        /// <summary>
        /// Removes a connection id to the users collection
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="connectionId">the connectionid to be removed from the users collection</param>
        /// <returns>Bool if the update was successfull</returns>
        Task<bool> RemoveUserConnection(string email, string connectionId);

    }
}
