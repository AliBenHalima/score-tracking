using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Constants = ScoreTracking.App.Helpers.Constants;
using UserGame = ScoreTracking.App.Models.UserGame;

namespace ScoreTracking.App.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly DatabaseContext _databaseContext;

        public GameService(IMapper mapper, IGameRepository gameRepository, IUserRepository userRepository, DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _databaseContext = databaseContext;
        }

        public async Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest)
        {
            using ( IDbContextTransaction transaction = _databaseContext.Database.BeginTransaction())
            {
                try
                {
                    Game game = _mapper.Map<Game>(createGameRequest);
                    Game newGame = await _gameRepository.Create(game);
                    IEnumerable<int> usersIds = createGameRequest.UserIds;
                    if (usersIds.Any())
                    {
                        IEnumerable<User> users = await _userRepository.FindByIds(usersIds);
                        if (users.Count() != usersIds.Count()) throw new BadRequestException("Error adding users to game");

                        await _gameRepository.AddPlayersToGame(newGame, users);
                    }
                    CreateGameDTO gameDTO = _mapper.Map<CreateGameDTO>(newGame);
                    transaction.Commit();
                    return gameDTO;
                }
                catch (ScoreTrackingException exception)
                {
                    transaction.Rollback();
                    throw new ScoreTrackingException(exception.Message, exception.StatusCode);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception("Error creating game");
                }
            }
        }

        public async Task AddPlayersToGame(int id, IEnumerable<int> playerIds)
        {
            Game game = await _gameRepository.GetGameWithPlayers(id);
            if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);

            IEnumerable<User> players = await _userRepository.FindByIds(playerIds);
            VerifyPlayersExistance(players, playerIds);
            VerifyGameMaxPlayersCount(game, game.UserGames.Count(), players.Count());
            VerifyPlayersAreNotGameMembers(players, game.UserGames);
            if (!IsGameStarted(game) && !IsGameCanceled(game)) throw new BadRequestException("Can't create game, verify game status");
            await _gameRepository.AddPlayersToGame(game, players);
        }
        public async Task<GameDetailsDTO> GetGame(int id)
        {
            GameDetailsDTO? game = await _gameRepository.GetGameDetailsById(id);
           return game;
        }
        public async Task StartGame(int id)
        {
            Game game = await _gameRepository.FindById(id);
            if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);

            if (IsGameStarted(game)) throw new BadRequestException($"Game already started at {game.StartedAt}");
            await _gameRepository.StartGame(game);
        }
        
        public async Task UpdateGameStatus(int id, GameStatus gameStatus)
        {
            Game game = await _gameRepository.FindById(id);
            if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);
            if (gameStatus == GameStatus.Started)
            {
                if (IsGameStarted(game)) throw new BadRequestException($"Game already started at {game.StartedAt}");
                if (IsGameCanceled(game)) throw new BadRequestException($"Game already Canceled at {game.CanceledAt}");

                await _gameRepository.StartGame(game);
            }
            if (gameStatus == GameStatus.Canceled)
            {
                if (IsGameCanceled(game)) throw new BadRequestException($"Game already canceled at {game.CanceledAt}");
                await _gameRepository.CancelGame(game);
            }

        }
        private void VerifyPlayersExistance(IEnumerable<User> players, IEnumerable<int> playerIds)
        {
            if (players.Count() != playerIds.Count())
                throw new BadRequestException("Can't add players to game, verify players ids");
        }
        private void VerifyGameMaxPlayersCount(Game game, int gamePlayersCount, int addedPlayersCount)
        {
            if (gamePlayersCount + addedPlayersCount > Constants.GameConstants.MaxPlayersCount)
                throw new BadRequestException(string.Format("Game {0} already has {1} players, can't add {2} more players", game.Name, game.UserGames.Count(), addedPlayersCount));
        }
        private void VerifyPlayersAreNotGameMembers(IEnumerable<User> players, IEnumerable<UserGame> gamePlayers)
        {
            IEnumerable<int> playerIds = players.Select(item => item.Id).ToList();
            bool gameHasPlayers = gamePlayers.Select(item => item.UserId).Intersect(playerIds).Any();
            if (gameHasPlayers) throw new BadRequestException("Player to be added is already a member in this game");
        }

        private bool IsGameStarted(Game game)
        {
            return game.StartedAt is not null;
        }
        private bool IsGameCanceled(Game game)
        {
            return game.CanceledAt is not null;
        }

    }
}
