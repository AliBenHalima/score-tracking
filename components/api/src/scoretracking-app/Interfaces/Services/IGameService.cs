using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IGameService
    {
         Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest);
         Task<GameDetailsDTO> GetGame(int id);
         Task AddPlayersToGame(int id, IEnumerable<int> playerIds);
         Task StartGame(int id);
         Task UpdateGameStatus(int id, GameStatus gameStatus);
         Task<IEnumerable<Game>> GetGamesByUser(int id);
         Task AddRound(int gameId, AddRoundRequest addRoundRequest);
        Task<Round> UpdateRoundScore(int gameId, int roundId, UpdateRoundRequest updateRoundRequest);
    }
}
