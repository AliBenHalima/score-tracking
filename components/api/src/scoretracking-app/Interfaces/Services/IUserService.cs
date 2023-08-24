using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IUserService
    {
        IQueryable<User> GetUsers(FilterDTO filters, CancellationToken cancellationToken);
         Task<User> GetUser(int id);
         Task<User> CreateUser(CreateUserRequest createUserRequest);
         Task<User> UpdateUser(int id, UpdateUserRequest updateUserRequest);
         Task DeleteUser(int id);
        Task TestQuartz();

    }
}
