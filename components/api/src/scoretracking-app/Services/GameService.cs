using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constants = ScoreTracking.App.Helpers.Constants;
using UserGame = ScoreTracking.App.Models.UserGame;

namespace ScoreTracking.App.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IMapper mapper, IGameRepository gameRepository, IUserRepository userRepository, IRoundRepository roundRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Game>> GetGamesByUser(int userId)
        {
            return await _gameRepository.GetGamesByUser(userId);
        }

        public async Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest)
        {
            using var transaction = _unitOfWork.BeginTransaction();

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
                await _unitOfWork.SaveChangesAsync();
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

        public async Task AddPlayersToGame(int id, IEnumerable<int> playerIds)
        {
            Game game = await _gameRepository.GetGameWithPlayers(id);
            if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);

            IEnumerable<User> players = await _userRepository.FindByIds(playerIds);
            VerifyPlayersExistance(players, playerIds);
            VerifyGameMaxPlayersCount(game, game.UserGames.Count(), players.Count());
            VerifyPlayersAreNotGameMembers(players, game.UserGames);
            if (IsGameStarted(game) && IsGameCanceled(game)) throw new BadRequestException("Can't create game, verify game status");
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

        //Rounds

        public async Task AddRound(int gameId, AddRoundRequest addRoundRequest)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            {
                try
                {
                    Game game = await _gameRepository.FindById(gameId);
                    if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);
                    IEnumerable<int> userIds = ExtractUsersFromRoundInformation(addRoundRequest.RoundInformation.ToList());
                    IEnumerable<int> gamePlayers = await _gameRepository.GameHasPlayers(gameId);
                    bool gameHasPlayers = gamePlayers.Any(userId => userIds.Contains(userId));
                    if (!gameHasPlayers) throw new BadRequestException("Error creating round, players are not game members");
                    if (IsGameCanceled(game)) throw new BadRequestException($"Can't add round, game was canceled at {0}", game.CanceledAt);
                    if (!IsGameStarted(game)) throw new BadRequestException($"Game must be started first");
                    if (IsGameEnded(game)) throw new BadRequestException($"Game already ended at {0}", game.EndedAt);
                    // Create Round

                    //int roundNumber = await _roundRepository.GetLatestRound();
                    Round? latestGameRound = await _gameRepository.GetGameLatestRound(gameId);

                    Round round = new Round
                    {
                        Number = (latestGameRound is null) ? Constants.RoundConstants.StartingNumber : ++latestGameRound.Number,
                        Status = addRoundRequest.Status
                    };
                    IEnumerable<UserGame> userGames = await _gameRepository.GetGameUsers(gameId);
                    Round createdRound = await _roundRepository.Create(round);
                    List<UserGameRound> userGameRounds = CreateUserGameRoundList(addRoundRequest.RoundInformation, userGames, createdRound, game);
                    await _roundRepository.AddRoundScores(round, userGameRounds);
                    bool GameHasPlayers = await GameHasPlayersLeft(game);
                    if (!GameHasPlayers) await _gameRepository.EndGame(game);
                    transaction.Commit();

                }
                catch (ScoreTrackingException exception)
                {
                    transaction.Rollback();
                    throw new ScoreTrackingException(exception.Message, exception.StatusCode);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception("Error adding round");
                }
            }
        }

        private async Task<bool> GameHasPlayersLeft(Game game)
        {
            IEnumerable<RoundSumDTO> roundSum = await _roundRepository.GetRoundSumScoresByPlayer(game.Id);
            int filteredRoundCount = roundSum.Where(r => r.Sum <= game.Score).ToList().Count(); // players with scoe <= max game score.
            return (filteredRoundCount >= Constants.GameConstants.MaxPlayersCount) ? true : false; // check if players left is > 3 

        }

        public async Task<Round> UpdateRoundScore(int gameId, int roundId, UpdateRoundRequest updateRoundRequest)
        {
            Game game = await _gameRepository.FindById(gameId);
            if (game is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Game).Name);
            Round round = await _roundRepository.FindById(roundId);
            if (round is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(Round).Name);

            return await _roundRepository.updateRoundScore(game, round, updateRoundRequest.RoundInformation);
        }

        private List<UserGameRound> CreateUserGameRoundList(IEnumerable<RoundInformationDTO> roundInformation, IEnumerable<UserGame> userGames, Round createdRound, Game game)
        {
            List<UserGameRound> userGameRounds = new List<UserGameRound>();
            foreach (RoundInformationDTO roundInfo in roundInformation)
            {
                int UserGameId = userGames.Where(ug => ug.UserId == roundInfo.UserId).First().Id;
                userGameRounds.Add(new UserGameRound()
                {
                    UserGameId = UserGameId,
                    RoundId = createdRound.Id,
                    Jokers = roundInfo.JokerCount,
                    Score = (roundInfo.Score + CalculateJokerAdditionalScore(game, roundInfo.JokerCount)),
                });
            }
            return userGameRounds;
        }

        private int CalculateJokerAdditionalScore(Game game, int jokerCount)
        {
            //Calculates the sum of jokers based on game options.
            if (game.HasJokerPenalty)
            {
                return (jokerCount * game.JokerPenaltyValue);
            }
            return 0;
        }

        private IEnumerable<int> ExtractUsersFromRoundInformation(List<RoundInformationDTO> roundInformation)
        {
            List<int> userIds = new List<int>();
            foreach (RoundInformationDTO round in roundInformation)
            {
                userIds.Add(round.UserId);
            }
            return userIds;
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
        private bool IsGameEnded(Game game)
        {
            return game.EndedAt is not null;
        }

    }
}
