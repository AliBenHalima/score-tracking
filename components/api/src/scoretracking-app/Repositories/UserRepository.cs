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
        public UserRepository(DatabaseContext databaseContext): base(databaseContext)
        { 
        }

        public override async Task<User> Create(User user)
        {
             databaseContext.users.Add(user);
             await this.databaseContext.SaveChangesAsync().ConfigureAwait(true);
             return user;
        }
    }
}
