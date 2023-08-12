using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests.Users;
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

namespace ScoreTracking.App.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.FindAll();
        }

        public async Task<User> GetUser(int id)
        {

            User? user = await _userRepository.FindByCondition(u => u.Id == id);

            if(user == null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);

            return user;

        }
        public async Task<User> CreateUser(CreateUserRequest createUserRequest)
        {
            if(await _userRepository.FindByEmail(createUserRequest.Email) is not null)
                throw new BadRequestException("Email already exists");

            if (await _userRepository.FindByPhone(createUserRequest.Phone) is not null)
                throw new BadRequestException("Phone number already exists");

            User user = this._mapper.Map<User>(createUserRequest);
             return  await _userRepository.Create(user); 
        }

        public async Task<User> UpdateUser(int id, UpdateUserRequest updateUserRequest)
        {
            User? userById = await _userRepository.FindByCondition(u => u.Id == id);
            if (userById is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);

            User? userByEmail = await _userRepository.FindByEmail(updateUserRequest.Email);
            if (userByEmail is not null && !GlobalHelper.AreIntegersEqual(userByEmail.Id, id)) throw new BadRequestException("Email already exists");

            User? userByPhone = await _userRepository.FindByPhone(updateUserRequest.Phone);
            if (userByPhone is not null && !GlobalHelper.AreIntegersEqual(userByPhone.Id, id)) throw new BadRequestException("Phone number already exists");

            return await _userRepository.Update(userById);
        }

        public async Task DeleteUser(int id)
        {
            User? user = await _userRepository.FindByCondition(u => u.Id == id);

            if (user is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);

             await _userRepository.Delete(user);
        }
    }
}
