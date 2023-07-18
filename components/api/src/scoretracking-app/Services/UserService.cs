using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Helpers;
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
        public DatabaseContext dbContext { get; }
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(DatabaseContext dbContext, IMapper mapper, IUserRepository userRepository)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
            this._userRepository = userRepository;
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
            if(await _userRepository.FindByCondition(u => u.Email == createUserRequest.Email) is not null)
                throw new BadRequestException("Email already exists");
            if (await _userRepository.FindByCondition(u => u.Phone == createUserRequest.Phone) is not null)
                throw new BadRequestException("Phone number already exists");
            User user = this._mapper.Map<User>(createUserRequest);
             return  await _userRepository.Create(user); 
        }

        public async Task<User> UpdateUser(int id, UpdateUserRequest updateUserRequest)
        {
            User user = await this.dbContext.users.FindAsync(id);
            if (user is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);
            if (await _userRepository.FindByCondition(u => (u.Email == updateUserRequest.Email) && (u.Id != id)) is not null)
                throw new BadRequestException("Email already exists");
            if (await _userRepository.FindByCondition(u => u.Phone == updateUserRequest.Phone && (u.Id != id)) is not null)
                if (user == null) throw new RessourceNotFoundException("Resource {0} Doesn't exist.", typeof(User).Name);
            this._mapper.Map(updateUserRequest, user);
            return await _userRepository.Update(user);
        }

        public async Task DeleteUser(int id)
        {
            User user = await this.dbContext.users.FindAsync(id);
            if (user is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);
             await _userRepository.Delete(user);
        }

    }
}
