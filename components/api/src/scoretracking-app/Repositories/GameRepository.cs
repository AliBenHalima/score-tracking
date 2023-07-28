﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<GameDetailsDTO> GetGameDetailsById(int id)
        {
            Game game = await Entity.Where(u => u.Id == id).AsNoTracking().FirstAsync();
            var gameWithRounds = DatabaseContext.Games
            .Where(g => g.Id == id)
            .Select(g => new GameDetailsDTO
            {
                Rounds = DatabaseContext.Rounds
                    .Where(r => r.UserGames.Any(x => x.GameId == id))
                    .Include(ug => ug.UserGameRounds)
                       .ThenInclude(r => r.UserGame)
                       .ThenInclude(u => u.User)
                    .ToList()
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

        public async Task AddPlayersToGame(Game game, IEnumerable<User> users)
        {
            foreach (User user in users)
            {
                game.Users.Add(user);
            }
            await DatabaseContext.SaveChangesAsync();
        }
        public async Task<Game> GetGameWithPlayers(int gameId)
        {
            return await Entity.Where(g => g.Id == gameId).Include(ug => ug.UserGames).FirstOrDefaultAsync();
        }

        public async Task StartGame(Game game)
        {
            game.StartedAt = DateTime.UtcNow;
            await DatabaseContext.SaveChangesAsync();
        }
        public async Task CancelGame(Game game)
        {
            game.CanceledAt = DateTime.UtcNow;
            await DatabaseContext.SaveChangesAsync();
        }

    }
}