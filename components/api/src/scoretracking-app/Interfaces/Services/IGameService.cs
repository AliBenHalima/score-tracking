using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IGameService
    {
        Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest);
    }
}
