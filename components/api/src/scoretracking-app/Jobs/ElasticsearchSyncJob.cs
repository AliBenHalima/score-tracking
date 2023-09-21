using AutoMapper;
using Quartz;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Elasticsearch;
using ScoreTracking.App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Jobs
{
    public class ElasticsearchSyncJob : IJob
    {
        private readonly UserRepository _userRepository;
        private readonly ISearchClient _searchClient;
        private readonly IMapper _mapper;

        public ElasticsearchSyncJob(UserRepository userRepository, ISearchClient searchClient, IMapper mapper)
        {
            _userRepository = userRepository;
            _searchClient = searchClient;
            _mapper = mapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dbUsers = await _userRepository.GetUsers();
            IEnumerable<UserElasticsearchDto> usersDto = _mapper.Map<IEnumerable<UserElasticsearchDto>>(dbUsers);
            var elasticUsers = await _searchClient.GetAll();

            var usersToAdd = usersDto.Where(user => !elasticUsers.Any(es => es.Id == user.Id)).ToList();
            var usersToDelete = elasticUsers.Where(user => !dbUsers.Any(es => es.Id == user.Id)).ToList();
            var usersToUpdate = usersDto.Where(user => elasticUsers.Any(es => es.Id == user.Id)).ToList();

            await _searchClient.BulkAdd(usersToAdd);
            await _searchClient.BulkDelete(usersToDelete);
            await _searchClient.BulkUpdate(usersToUpdate);



            //var pageNumber = 0;
            //var users = new List<User>();
            //var hasMoreUsers = true;

            //while (hasMoreUsers)
            //{
            //    var batch = await _dbContext.Users
            //        .OrderBy(u => u.Id) // Replace with your ordering criteria
            //        .Skip(pageNumber * batchSize)
            //        .Take(batchSize)
            //        .ToListAsync();

            //    if (batch.Any())
            //    {
            //        users.AddRange(batch);
            //        pageNumber++;
            //    }
            //    else
            //    {
            //        hasMoreUsers = false;
            //    }
            //}

            //return users;
        }
    }
}
