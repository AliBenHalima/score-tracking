using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.Database;
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

        public override async Task<Game> Create(Game game)
        {
            DatabaseContext.Games.Add(game);
            await DatabaseContext.SaveChangesAsync();
            return game;
        }

        public async Task AddGameMembers(Game game, IEnumerable<User> users) 
        {
            foreach (User user in users)
            {
                game.Users.Add(user);
            }
             await DatabaseContext.SaveChangesAsync();
        }
    }
}