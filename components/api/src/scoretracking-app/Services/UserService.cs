using AutoMapper;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Jobs;
using ScoreTracking.App.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public readonly ILogger<UserService> _logger;
        public readonly IApplicationHelper _helper;
        public UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger, IApplicationHelper helper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _helper = helper;
        }

        public IQueryable<User> GetUsers(FilterDTO filters, CancellationToken cancellationToken)
        {
            IQueryable<User> userQuery =  _userRepository.FindAll(filters, cancellationToken);
            return userQuery;
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
            if (userByEmail is not null && !_helper.AreIntegersEqual(userByEmail.Id, id)) 
                throw new BadRequestException("Email already exists");

            User? userByPhone = await _userRepository.FindByPhone(updateUserRequest.Phone);
            if (userByPhone is not null && !_helper.AreIntegersEqual(userByPhone.Id, id)) throw new BadRequestException("Phone number already exists");

            User user = _mapper.Map<User>(updateUserRequest);

            return await _userRepository.Update(user);
        }

        public async Task DeleteUser(int id)
        {
            User? user = await _userRepository.FindByCondition(u => u.Id == id);

            if (user is null) throw new RessourceNotFoundException("{0} Doesn't exist.", typeof(User).Name);

             await _userRepository.Delete(user);
        }

        public async Task TestQuartz()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();

            IJobDetail job = JobBuilder.Create<TestJob>()
            .WithIdentity("myJob", "group1")
            .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
               .WithIntervalInSeconds(40)
               .RepeatForever())
              .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
