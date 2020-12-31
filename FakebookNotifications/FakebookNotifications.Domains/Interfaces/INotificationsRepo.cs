using FakebookNotifications.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess.Models.Interfaces
{ 
    public interface INotificationsRepo
    {
        /// <summary>
        /// Get all notifications from each user.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetAllNotifications();
        /// <summary>
        ///  Get All Notifications by a person's email
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetNotificationsByEmail(Notification notification);
        /// <summary>
        ///  Add a Notification to the databse
        /// </summary>
        /// <returns> A bool that let's us know if we passed or failed</returns>
        Task<bool> AddNotification();
        /// <summary>
        /// Delete a Notification from the database
        /// </summary>
        /// <returns> A bool that let's us know if we passed or failed</returns>
        Task<bool> DeleteNotification();
    }
}
