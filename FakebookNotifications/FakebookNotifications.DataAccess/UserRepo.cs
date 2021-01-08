using FakebookNotifications.DataAccess.Models;
using FakebookNotifications.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakebookNotifications.DataAccess
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
            Domain.Models.User domainUser = new Domain.Models.User(dbUser.Id, dbUser.Email);
            return domainUser;
        }

        public async Task<bool> CreateUserAsync(Domain.Models.User user)
        {
            try
            {
                // Create an entity version of the user.
                User newuser = new User()
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

        public Task<IEnumerable<Domain.Models.User>> GetUsersSubscriptionsByIdAsync(string email)
        {
            throw new NotImplementedException();
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
                User updatedUser = new User()
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
            // Find the user entity, based on the user email.
            IAsyncCursor<User> findUser = await _dbCollection.FindAsync(u => u.Email == user.Email);
            User foundUser = findUser.FirstOrDefault();

            // Return the count of their notifications.
            return foundUser.Notifications.Count();

        }

        public async Task<IEnumerable<Domain.Models.Notification>> GetUserNotificationsAsync(Domain.Models.User user)
        {
            // Find the user entity, based on the user email.
            IAsyncCursor<User> findUser = await _dbCollection.FindAsync(u => u.Email == user.Email);

            // Create a list of Notification Entities
            IEnumerable<Notification> foundNotifications = findUser.FirstOrDefault().Notifications;

            // Select each item from the list and create a new notification entity.
            IEnumerable<Domain.Models.Notification> notificationList = foundNotifications.Select(x => new Domain.Models.Notification()
            {
                Id = x.Id,
                Type = x.Type,
                Date = (DateTime)x.Date,
                HasBeenRead = x.HasBeenRead,
                LoggedInUserId = x.LoggedInUserId,
                TriggerUserId = x.TriggerUserId
            }).AsEnumerable(); // Create this item as an Enumarable

            return notificationList;
        }

        public async Task<Domain.Models.User> AddUserSubscriptionAsync(string subscriberEmail, string subscribeeEmail)
        {
            Domain.Models.User subscriber = await GetUserAsync(subscriberEmail);
            Domain.Models.User subscribee = await GetUserAsync(subscribeeEmail);
            subscriber.Follows.Add(subscribeeEmail);
            subscribee.Subscribers.Add(subscriberEmail);

            Domain.Models.Notification notification = new Domain.Models.Notification()
            {
                Type = new KeyValuePair<string, int>("follow", 0),
                Date = DateTime.Now,
                HasBeenRead = false,
                LoggedInUserId = subscribeeEmail,
                TriggerUserId = subscriberEmail
            };

            //add follow notification to database
            var result = await _noteRepo.CreateNotificationAsync(notification);
            if(result == true)
            {
                return subscribee;
            }
            else
            {
                throw new Exception("Error creating follow notification");
            }
        }
    }
}