using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
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

namespace ScoreTracking.App.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public GameService(IMapper mapper, IGameRepository gameRepository, IUserRepository userRepository )
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _userRepository = userRepository;       
        }

        public async Task<CreateGameDTO> CreateGame(CreateGameRequest createGameRequest)
        {
                Game game = _mapper.Map<Game>(createGameRequest);
                Game newGame = await _gameRepository.Create(game);
                IEnumerable<int> usersIds = createGameRequest.UserIds;
                if (usersIds.Any())
                {
                    IEnumerable<User> users = await _userRepository.FindByIds(usersIds);
                    await _gameRepository.AddGameMembers(newGame, users);
                }

                CreateGameDTO gameDTO = _mapper.Map<CreateGameDTO>(newGame);
                return gameDTO;
        }

    }
}
