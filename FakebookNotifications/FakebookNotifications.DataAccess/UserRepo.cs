﻿using FakebookNotifications.DataAccess.Models;
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
            var p = await _dbCollection.FindAsync<User>(u => u.Email == email).FirstOrDefault();
            var newuser = Domain.Models.User();
            return newuser;
        }

        public Task<bool> CreateUserAsync(Domain.Models.User user)
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
