using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class UserRepo : IUserRepo
    {
        private readonly NotificationsContext _context;

        public UserRepo(NotificationsContext context)
        {
            _context = context;
        }

        public Task<Domain.Models.User> GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUserAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Models.User>> GetUsersSubscriptionsByIdAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalUserNotificationsAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Models.Notification>> GetUserNotificationsAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }
    }
}
