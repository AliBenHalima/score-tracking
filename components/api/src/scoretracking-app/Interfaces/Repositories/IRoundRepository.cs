using ScoreTracking.App.DTOs;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IRoundRepository : IBaseRepository<Round>
    {
        Task<Round> AddRoundScores(Round round, List<UserGameRound> userGameRounds);
        Task<Round> updateRoundScore(Game game, Round round, IEnumerable<RoundInformationDTO> roundInformation);
        Task<IEnumerable<RoundSumDTO>> GetRoundSumScoresByPlayer(int gameId);
    }
}