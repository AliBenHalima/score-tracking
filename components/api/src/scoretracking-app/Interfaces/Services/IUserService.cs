using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUser(int id);
        public Task<User> CreateUser(CreateUserRequest createUserRequest);
        public Task<User> UpdateUser(int id, UpdateUserRequest updateUserRequest);
        public Task DeleteUser(int id);
        public Task<IEnumerable<Game>> GetUserGames(int id);

    }
}
