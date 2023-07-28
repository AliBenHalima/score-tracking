using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/games/{id}/rounds")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IRoundService _roundService;

       public RoundController(IRoundService roundService) {
            _roundService= roundService;
        }

        [HttpPost]
        public async Task<Round> UpdateRoundScores(int id, AddRoundRequest addRoundRequest)
        {
            throw new NotImplementedException();
        }

    }
}
