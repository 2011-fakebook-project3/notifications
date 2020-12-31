using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.DataAccess.Models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
{
    class UserRepo : IUserRepo
    {
        public Task<bool> CreateUser(Domains.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(Domains.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<Domains.Models.User> GetUser(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domains.Models.Notification>> GetUserNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domains.Models.User>> GetUsersBySubscription(List<string> Ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalNotifications(Domains.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(Domains.Models.User user)
        {
            throw new NotImplementedException();
        }
    }
}
