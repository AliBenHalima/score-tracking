using Microsoft.EntityFrameworkCore;
using ScoreTracking.Models;
using ScoreTracking.Requests;

namespace ScoreTracking.Interfaces.Services
{
    public interface IUserService
    {
        public  Task<List<User>> GetUsers();
        public  Task<User> GetUser(int id);
        public  Task<User> CreateUser(CreateUserRequest createUserRequest);
        public  Task<User> UpdateUser(int id, UpdateUserRequest updateUserRequest);
        public  Task DeleteUser(int id);
   
    }
}
