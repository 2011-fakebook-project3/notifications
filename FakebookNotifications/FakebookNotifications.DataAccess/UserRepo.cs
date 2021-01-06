using FakebookNotifications.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class UserRepo
    {
        public Task<User> GetUserAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersBySubscriptionAsync(List<string> Ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalUserNotificationsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetUserNotificationsAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
