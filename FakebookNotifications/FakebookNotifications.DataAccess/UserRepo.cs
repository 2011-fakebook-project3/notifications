using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    public class UserRepo : IUserRepo
    {
        private readonly INotificationsContext _context;
        private readonly IMongoCollection<User> _dbCollection;
        public UserRepo(INotificationsContext context)
        {
            _context = context;
            _dbCollection = _context.User;
        }

        public async Task<Domain.Models.User> GetUserAsync(string email)
        {
            var user = await _dbCollection.FindAsync(u => u.Email == email);
            var dbUser = user.FirstOrDefault();

            var domainUser = new Domain.Models.User(dbUser.Id, dbUser.Email);
            return domainUser;
        }

        public async Task<bool> CreateUserAsync(Domain.Models.User user)
        {
            try
            {
                User newuser = new User()
                {
                    Id = user.Id,
                    Email = user.Email
                };
                await _dbCollection.InsertOneAsync(newuser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<IEnumerable<Domain.Models.User>> GetUsersSubscriptionsByIdAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(Domain.Models.User user)
        {
            try
            {
                var id = user.Id;
                await _dbCollection.DeleteOneAsync(Builders<User>.Filter.Eq("Id", id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Domain.Models.User user)
        {
            try
            {
                User updatedUser = new User()
                {
                    Id = user.Id,
                    Email = user.Email
                };
                await _dbCollection.ReplaceOneAsync(u => u.Id == updatedUser.Id, updatedUser);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<int> TotalUserNotificationsAsync(Domain.Models.User user)
        {
            var findUser = await _dbCollection.FindAsync(u => u.Email == user.Email);
            var foundUser = findUser.FirstOrDefault();
            return foundUser.Notifications.Count();
        }

        public Task<IEnumerable<Domain.Models.Notification>> GetUserNotificationsAsync(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }
    }
}
