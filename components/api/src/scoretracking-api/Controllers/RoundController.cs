using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/games/{gameId}/rounds")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IGameService _gameService;

        public RoundController(IGameService gameService)
        {
            _gameService = gameService;
        }
         
        [HttpPost]
        public async Task<ActionResult<SuccessResponse>> AddRound(int gameId, AddRoundRequest addRoundRequest)
        {
             await _gameService.AddRound(gameId, addRoundRequest);
            return Ok(new SuccessResponse($"Round Created"));
        }

        [HttpPut]
        [Route("{roundId}")]
        public async Task<ActionResult<GenericSuccessResponse<Round>>> UpdateRoundScore(int gameId, int roundId, UpdateRoundRequest updateRoundRequest)
        {
             Round round = await _gameService.UpdateRoundScore(gameId, roundId, updateRoundRequest);
            return Ok(new GenericSuccessResponse<Round>($"Round Updated", round));

        }

    }
}
