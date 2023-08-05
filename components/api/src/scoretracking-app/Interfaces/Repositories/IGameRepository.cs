using ScoreTracking.App.DTOs;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        public Task<Game> Create(Game game);
        public Task AddPlayersToGame(Game game, IEnumerable<User> users);
        public Task<GameDetailsDTO> GetGameDetailsById(int id);
        public Task<Game> GetGameWithPlayers(int gameId);
        public Task StartGame(Game game);
        public Task CancelGame(Game game);
    }
}