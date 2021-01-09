using FakebookNotifications.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakebookNotifications.Domain.Interfaces
{
    public interface INotificationsRepo
    {
        /// <summary>
        /// Get all notifications from each user.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an IEnumerable of all Notifications.
        /// </returns>
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        /// <summary>
        ///  Create a Notification and add to the databse.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful create.
        /// </returns>
        Task<bool> CreateNotificationAsync(Domain.Models.Notification notification);
        /// <summary>
        /// Delete a Notification from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful delete.
        /// </returns>
        Task<bool> DeleteNotificationAsync(Domain.Models.Notification notification);
        /// <summary>
        /// Updates a notification in the database
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a bool that returns true on a successful delete.
        /// </returns>
        Task<bool> UpdateNotificationAsync(Domain.Models.Notification notification);
        /// <summary>
        /// Method to get all unread notifications
        /// </summary>
        /// <param name="userEmail">email to search notifications for as string</param>
        /// <returns>A list of notifications</returns>
        Task<List<Notification>> GetAllUnreadNotificationsAsync(string userEmail);
        /// <summary>
        /// Method to get unread notification count
        /// </summary>
        /// <param name="userEmail">email to search notifications for as string</param>
        /// <returns>Unread notification count as an int</returns>
        Task<int> GetTotalUnreadNotificationsAsync(string userEmail);
    }


}
