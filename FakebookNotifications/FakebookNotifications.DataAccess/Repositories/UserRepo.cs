using System;
using System.Linq;
using System.Threading.Tasks;
using FakebookNotifications.DataAccess.Interfaces;
using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;

namespace FakebookNotifications.DataAccess.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly INotificationsContext _context;
        private readonly IMongoCollection<User> _dbCollection;
        private readonly INotificationsRepo _noteRepo;

        public UserRepo(INotificationsContext context, INotificationsRepo noteRepo)
        {
            _context = context;
            _dbCollection = _context.User;
            _noteRepo = noteRepo;
        }

        public async Task<Domain.Models.User> GetUserAsync(string email)
        {
            // Find the user by their email.
            IAsyncCursor<User> user = await _dbCollection.FindAsync(u => u.Email == email);
            User dbUser = user.FirstOrDefault();

            // Create the domain model version of the user and return it.
            Domain.Models.User domainUser = new()
            {
                Id = dbUser.Id,
                Email = dbUser.Email,
                Connections = dbUser.Connections
            };
            return domainUser;
        }

        public async Task<bool> CreateUserAsync(Domain.Models.User user)
        {
            try
            {
                // Create an entity version of the user.
                User newuser = new()
                {
                    Email = user.Email
                };
                // Insert it into the database, then return true if successful.
                await _dbCollection.InsertOneAsync(newuser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Domain.Models.User user)
        {
            try
            {
                // Grab an identifier.
                string email = user.Email;
                // Remove the user from the database. Return true if it succeeds.
                await _dbCollection.DeleteOneAsync(Builders<User>.Filter.Eq("Email", email));
                return true;
            }
            catch
            {
                // Return false if deleting the user fails.
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Domain.Models.User user)
        {
            try
            {
                // Create an entity version of the user.
                User updatedUser = new()
                {
                    Id = user.Id,
                    Email = user.Email
                };
                // Replace the user information with the information from domain. Return true if it succeeds.
                await _dbCollection.ReplaceOneAsync(u => u.Id == updatedUser.Id, updatedUser);
                return true;
            }
            catch
            {
                // Return false if updating the user fails.
                return false;
            }
        }

        public async Task<int> TotalUserNotificationsAsync(Domain.Models.User user)
        {
            var notifications = await _noteRepo.GetTotalUnreadNotificationsAsync(user.Email);

            // Return the count of their notifications.
            return notifications;

        }

        public async Task<bool> AddUserConnection(string email, string connectionId)
        {
            //Get the user
            IAsyncCursor<User> user = await _dbCollection.FindAsync(u => u.Email == email);
            User dbUser = user.FirstOrDefault();

            if (user != null)
            {
                try
                {
                    //Add the connection id to their colelction
                    dbUser.Connections.Add(connectionId);
                    await _dbCollection.ReplaceOneAsync(u => u.Email == dbUser.Email, dbUser);
                    return true;
                }
                catch
                {
                    throw new Exception("Error adding connection id to user");
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserConnection(string email, string connectionId)
        {
            //Get the user
            IAsyncCursor<User> user = await _dbCollection.FindAsync(u => u.Email == email);
            User dbUser = user.FirstOrDefault();

            try
            {
                //Remove the connection id to their colelction
                dbUser.Connections.Remove(connectionId);
                await _dbCollection.ReplaceOneAsync(u => u.Email == dbUser.Email, dbUser);
                return true;
            }
            catch
            {
                throw new Exception("Error removing connection id to user");
            }
        }
    }
}
