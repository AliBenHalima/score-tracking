using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Providers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Services
{
    public class AuthenticationService : Interfaces.Services.IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticationService(IMapper mapper, IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<User> Register(RegisterUserRequest registerUserRequest) {
            User? userByEmail = await _userRepository.FindByEmail(registerUserRequest.Email);
            if (userByEmail is not null) { throw new RessourceNotFoundException("{0} Already Exist", nameof(RegisterUserRequest.Email));}
            User user = _mapper.Map<User>(registerUserRequest);
            user.Password = PasswordManager.HashPassword(registerUserRequest.Password);
            await _userRepository.Create(user);
            return user;
        }

        public async Task<string> Signin(SigninUserRequest signinUserRequest)
        {
           User? user = await _userRepository.FindByEmail(signinUserRequest.Email);
            if (user is null) throw new BadRequestException("Invalid Credentials");
           bool IsPasswordIdentical = PasswordManager.VerifyPassword(user.Password, signinUserRequest.Password);
            if (!IsPasswordIdentical) throw new BadRequestException("Invalid Credentials");

            string token = _jwtProvider.Generate(user);
            return token;
        }
    }
}
