using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGameRepository _gameRepository;

        public GameController(IGameService gameService, IGameRepository gameRepository)
        {
            _gameService = gameService;
            _gameRepository = gameRepository;
        }

        [HttpPost]
        [Route("games")]
        public async Task<IActionResult> CreateGame(CreateGameRequest createGameRequest)
        {
           CreateGameDTO gameDTO = await _gameService.CreateGame(createGameRequest);
           
            return Ok(new SuccessResponse<CreateGameDTO>("Game Created", gameDTO));
        }
    }
}
