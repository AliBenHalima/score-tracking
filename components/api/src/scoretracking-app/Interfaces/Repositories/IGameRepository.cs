using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        public Task<Game> Create(Game game);
        public Task AddGameMembers(Game game, IEnumerable<User> users);
    }
}