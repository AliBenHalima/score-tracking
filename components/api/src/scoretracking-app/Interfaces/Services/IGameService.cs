using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IGameService
    {
        public Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest);
        public Task<GameDetailsDTO> GetGame(int id);
        public Task AddPlayersToGame(int id, IEnumerable<int> playerIds);
        public Task StartGame(int id);
        public Task UpdateGameStatus(int id, GameStatus gameStatus);
    }
}
