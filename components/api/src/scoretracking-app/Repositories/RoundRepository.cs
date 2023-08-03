using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public class RoundRepository : BaseRepository<Round>, IRoundRepository
    {
        public RoundRepository(DatabaseContext databaseContext) : base(databaseContext) { }


        public override async Task<Round> FindById(int id)
        {
            return await Entity.Where(r => r.Id == id).Include(r => r.UserGameRounds).ThenInclude(r => r.UserGame).FirstOrDefaultAsync();
        }

        public override async Task<Round> Create(Round round)
        {
            Entity.Add(round);
            await DatabaseContext.SaveChangesAsync();
            return round;
        }
        public async Task<Round> AddRoundScores(Round round, List<UserGameRound> userGameRounds)
        {

            foreach(UserGameRound userGameRound in userGameRounds)
            {
                     round.UserGameRounds.Add(userGameRound);
            }
             await DatabaseContext.SaveChangesAsync();
            return round;
        }

        public async Task<Round> updateRoundScore(Game game, Round round, IEnumerable<RoundInformationDTO> roundInformation)
        {
            foreach (UserGameRound userGameRound in round.UserGameRounds)
            {
                RoundInformationDTO filteredRound = FilterRoundInformation(roundInformation, userGameRound.UserGame.UserId);
                userGameRound.Jokers = filteredRound.JokerCount;
                userGameRound.Score = (filteredRound.Score + CalculateJokerAdditionalScore(game, filteredRound.JokerCount));
            }
            await DatabaseContext.SaveChangesAsync();
            return round;
        }

        private RoundInformationDTO FilterRoundInformation(IEnumerable<RoundInformationDTO> roundInformation, int userId)
        {
            return roundInformation.Where(r => r.UserId == userId).First();
        }

        public async Task<IEnumerable<RoundSumDTO>> GetRoundSumScoresByPlayer(int gameId)
        {
           var data = await Entity
                    .Where(ug => ug.UserGames.Any(x=> x.GameId == gameId))
                    .SelectMany(ug => ug.UserGameRounds)
                    .GroupBy(ugr => ugr.UserGame.UserId)
                    .Select(g => new RoundSumDTO
                    {
                        UserId = g.Key,
                        Sum = g.Sum(g => g.Score)
                    })
                    .ToListAsync();
            return data;
        }
        private int CalculateJokerAdditionalScore(Game game, int jokerCount)
        {
            //Calculates the sum of jokers based on game options. example if joker count = 2 and joker penalty value is 50 then the resul will be equal to 100.
            if (game.HasJokerPenalty)
            {
                return (jokerCount * game.JokerPenaltyValue);
            }
            return 0;
        }
    }

   
}