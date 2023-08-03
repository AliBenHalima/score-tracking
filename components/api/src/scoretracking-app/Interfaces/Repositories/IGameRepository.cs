using ScoreTracking.App.DTOs;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game> Create(Game game);
        Task AddPlayersToGame(Game game, IEnumerable<User> users);
        Task<GameDetailsDTO> GetGameDetailsById(int id);
        Task<Game> GetGameWithPlayers(int gameId);
        Task StartGame(Game game);
        Task EndGame(Game game);
        Task CancelGame(Game game);
        Task<IEnumerable<Game>> GetGamesByUser(int userId);
        Task<List<int>> GameHasPlayers(int gameId);
        Task<Round?> GetGameLatestRound(int gameId);
        Task<IEnumerable<UserGame>> GetGameUsers(int gameId);
    }}