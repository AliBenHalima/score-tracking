using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/games")]
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

        [HttpGet]
        [Route("users/{id}")]
        public async Task<ActionResult<GenericSuccessResponse<List<Game>>>> GetGamesByUser([FromRoute] int id)
        {
            IEnumerable<Game> games = await this._gameService.GetGamesByUser(id);
            return Ok(new GenericSuccessResponse<IEnumerable<Game>>("Game Fetched", games));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GenericSuccessResponse<GameDetailsDTO>>> GetGame([FromRoute] int id, DatabaseContext databaseContext)
        {


            GameDetailsDTO gameDetails = await this._gameService.GetGame(id);
            return Ok(new GenericSuccessResponse<GameDetailsDTO>("Game Fetched", gameDetails));
        }


        [HttpPost]
        public async Task<ActionResult<GenericSuccessResponse<CreateGameDTO>>> CreateGame(CreateGameRequest createGameRequest)
        {
           CreateGameDTO gameDTO = await _gameService.CreateGame(createGameRequest);
           
            return Ok(new GenericSuccessResponse<CreateGameDTO>("Game Created", gameDTO));
        }

        [HttpPost]
        [Route("{id}/add-players")]
        public async Task<ActionResult<SuccessResponse>> AddPlayersToGame(int id, AddPlayersToGameRequest addPlayersToGameRequest)
        {
            await this._gameService.AddPlayersToGame(id, addPlayersToGameRequest.PlayerIds);
            return Ok(new SuccessResponse("Game players added successfully"));
        }

        [HttpPut]
        [Route("{id}/update-status")]
        public async Task<ActionResult<SuccessResponse>> UpdateGameStatus(int id, UpdateGameStatusRequest updateGameStatusRequest)
        {
            await this._gameService.UpdateGameStatus(id, updateGameStatusRequest.Status) ;
            return Ok(new SuccessResponse($"game {updateGameStatusRequest.Status} "));
        }



    }
}
