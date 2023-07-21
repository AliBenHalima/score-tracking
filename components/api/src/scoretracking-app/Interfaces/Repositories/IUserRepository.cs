using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindByEmail(string email);
        Task<User> FindByPhone(string phone);
        Task<IEnumerable<Game>> GetGamesByUser(int userId);


    }

}
