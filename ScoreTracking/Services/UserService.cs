using AutoMapper;
using Common.WebApi.Database;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.Interfaces.Services;
using ScoreTracking.Models;
using ScoreTracking.Requests;

namespace ScoreTracking.Services
{
    public class UserService : IUserService
    {
        public DatabaseContext dbContext { get; }

        private readonly IMapper _mapper;
        public UserService(DatabaseContext dbContext, IMapper mapper) {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<List<User>> GetUsers()
        {
            return await dbContext.users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await dbContext.users.FindAsync(id);
        }
        public async Task<User> CreateUser(CreateUserRequest createUserRequest)
        {
            User user = this._mapper.Map<User>(createUserRequest);
            dbContext.users.Add(user);
            await this.dbContext.SaveChangesAsync().ConfigureAwait(true);
            return user;
        }

        public async Task<User> UpdateUser(int id , UpdateUserRequest updateUserRequest)
        {
            User user = await this.dbContext.users.FindAsync(id);
            this._mapper.Map(updateUserRequest, user);
            dbContext.users.Update(user);
            await this.dbContext.SaveChangesAsync().ConfigureAwait(true);
            return user;
        }

       public async Task DeleteUser(int id)
        {
            User? user = await dbContext.users.FindAsync(id);
            if (user != null)
            {
                dbContext.users.Remove(user);
                dbContext.SaveChanges();
            }
        }

    }
}
