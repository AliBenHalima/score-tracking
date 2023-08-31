using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<IEnumerable<Game>> GetGamesByUser(int userId)
        {
            IEnumerable<Game> games = await DatabaseContext.Users
            .Where(u => u.Id == userId)
            .SelectMany(ug => ug.UserGames)
            .Select(g => g.Game)
            .ToListAsync();
            return games;
        }
        public async Task<GameDetailsDTO> GetGameDetailsById(int id)
        {
            Game game = await Entity.Where(u => u.Id == id).AsNoTracking().FirstAsync();
            var gameWithRounds = DatabaseContext.Games
            .Where(g => g.Id == id)
            .Select(g => new GameDetailsDTO
            {
                Id = g.Id,
                Name = g.Name,
                Code = g.Code,
                Score = g.Score,
                EndingType = g.EndingType,
                StartedAt = g.StartedAt,
                EndedAt = g.EndedAt,
                CanceledAt = g.CanceledAt,
                HasJokerPenalty = g.HasJokerPenalty,
                JokerPenaltyValue = g.JokerPenaltyValue,
                Rounds = DatabaseContext.Rounds
                    .Where(r => r.UserGames.Any(x => x.GameId == id))
                    .Include(ug => ug.UserGameRounds)
                       .ThenInclude(r => r.UserGame)
                       .ThenInclude(u => u.User)
                    .ToList(),
                RoundSum = DatabaseContext.Games
                .Where(g => g.Id == id)
                .SelectMany(g => g.UserGames)
                  .SelectMany(g => g.UserGameRounds)
                   .GroupBy(gr => gr.UserGame.UserId)
                .Select(s => new RoundSumDTO
                {
                    UserId = s.Key,
                    Sum = s.Sum(ugr => ugr.Score)
                }).ToList()
            })
            .FirstOrDefault();

            return gameWithRounds;

        }

        public override async Task<Game> Create(Game game)
        {
            DatabaseContext.Games.Add(game);
            await DatabaseContext.SaveChangesAsync();
            return game;
        }

        public Task AddPlayersToGame(Game game, IEnumerable<User> users)
        {
            foreach (User user in users)
            {
                game.Users.Add(user);
            }
           return Task.CompletedTask;
        }
        public async Task<Game> GetGameWithPlayers(int gameId)
        {
            return await Entity.Where(g => g.Id == gameId).Include(ug => ug.UserGames).FirstOrDefaultAsync();
        }

        public async Task<List<int>> GameHasPlayers(int gameId)
        {
            // Check if userIds belong to the requested game.
            return await DatabaseContext.Set<Game>().Where(g => g.Id == gameId)
                .SelectMany(g => g.UserGames)
                .Select(ug => ug.UserId)
                .Distinct()
                .ToListAsync();
        }

        public Task StartGame(Game game)
        {
            game.StartedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }
        public Task EndGame(Game game)
        {
            game.EndedAt = DateTime.UtcNow;
            game.EndingType = GameEndingType.Default;
            return Task.CompletedTask;
        }
        public Task CancelGame(Game game)
        {
            game.CanceledAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<Round?> GetGameLatestRound(int gameId)
        {
              return await Entity
             .Where(g => g.Id == gameId)
             .SelectMany(g => g.UserGames)
             .SelectMany(ug => ug.Rounds)
             .OrderByDescending(r => r.CreatedAt)
             .AsNoTracking()
             .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<UserGame>> GetGameUsers(int gameId)
        {
           return await Entity
                .Where(g => g.Id == gameId)
                .SelectMany(g => g.UserGames)
                .ToListAsync();
        }

    }
}