using FakebookNotifications.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess.Models.Interfaces
{
    interface IUserRepo
    {
        /// <summary>
        /// Gets a User
        /// </summary>
        /// <returns> A user with it's email and Id</returns>
        Task<User> GetUser();
        /// <summary>
        /// Creates a User
        /// </summary>
        /// <returns>A bool that let's us know if the project passed or fail</returns>
        Task<bool> CreateUser();
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <returns> A bool that lets us know if it passed or failed </returns>
        Task<bool> DeleteUser();
    }
}
