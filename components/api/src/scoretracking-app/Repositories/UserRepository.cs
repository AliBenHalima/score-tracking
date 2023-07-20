using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ScoreTracking.App.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        protected DbSet<User> test;

        public UserRepository(DatabaseContext databaseContext): base(databaseContext)
        {
        }

        public override async Task<User> Create(User user)
        {
            DatabaseContext.Users.Add(user);
            await DatabaseContext.SaveChangesAsync();
             return user;
        }
        public async Task<User> FindByEmail(string email)
        {
            return await Entity.Where(u => u.Email == email).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<User> FindByPhone(string phone)
        {
            return await Entity.Where(u => u.Phone == phone).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
