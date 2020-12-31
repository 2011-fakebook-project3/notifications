using FakebookNotifications.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess.Models.Interfaces
{
    public interface IUserRepo
    {
        /// <summary>
        /// Gets a User
        /// </summary>
        /// <returns> A user with it's email and Id</returns>
        Task<User> GetUser(int Id);
        /// <summary>
        /// Creates a User
        /// </summary>
        /// <returns>A bool that let's us know if the project passed or fail</returns>
        Task<bool> CreateUser(User user);
        /// <summary>
        /// Return a list of users from a list of ids. 
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns> An IEnumerable of the user's followees  </returns>
        Task<IEnumerable<User>> GetUsersBySubscription(List<string> Ids);
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <returns> A bool that lets us know if it passed or failed </returns>
        Task<bool> DeleteUser(User user);
        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns> A bool that let's us know if it passed or failed</returns>
        Task<bool> UpdateUser(User user);
        /// <summary>
        /// Returns the amount of total notifications a user has
        /// </summary>
        /// <param name="user"></param>
        /// <returns> An int to represent unread notifications</returns>
        Task<int> TotalNotifications(User user);
        /// <summary>
        /// Get 5 notifications at a time from a user.
        /// Should be called with page number, if page number is -1. Return all notifications at once.
        /// </summary>
        /// <returns> An IEnumerable of User Notifications.</returns>
        Task<IEnumerable<Notification>> GetUserNotifications();

    }
}
