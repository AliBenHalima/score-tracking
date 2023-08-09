using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Macs;
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
    [SimpleJob(RuntimeMoniker.Net70)]
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
        [Benchmark]
        [Arguments("AliBeHalima69@gmail.com")]
        public async Task<User?> FindByEmail(string email)
        {
            return await Entity.Where(u => u.Email == email).AsNoTracking().FirstOrDefaultAsync();
        }
        [Benchmark]
        [Arguments("+216540142572")]

        public async Task<User?> FindByPhone(string phone)
        {
            return await Entity.Where(u => u.Phone == phone).AsNoTracking().FirstOrDefaultAsync();
        }

    }
}
