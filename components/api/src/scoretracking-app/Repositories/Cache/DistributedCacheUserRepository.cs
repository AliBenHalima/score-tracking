using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Extensions.Query;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Cache;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories.Cache
{
    public class DistributedCacheUserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserRepository _decorated;
        private readonly ICacheService _cacheService;
        public DistributedCacheUserRepository(DatabaseContext databaseContext, UserRepository userRepository, ICacheService cacheService) : base(databaseContext)
        {
            _decorated = userRepository;
            _cacheService = cacheService;
        }

        public override async Task<PagedList<User>?> FindAll(FilterDTO filters, CancellationToken cancellationToken)
        {
            string key = $"users-{filters.Page}-{filters.PageSize}-{filters.SearchTerm}-{filters.SortOrder}-{filters.SortOrder}";
            PagedList<User>? data = await _cacheService.GetAsync<PagedList<User>?>(key,
              () => _decorated.FindAll(filters, cancellationToken),
               cancellationToken);
            return data;
        }

        public override Task<User> Create(User user)
        {
            _decorated.Create(user);
            return Task.FromResult(user);
        }

        [Benchmark]
        [Arguments("AliBeHalima61@gmail.com")]
        public async Task<User?> FindByEmail(string email)
        {
            return await _decorated.FindByEmail(email);
        }

        [Benchmark]
        [Arguments("+216540142572")]

        public async Task<User?> FindByPhone(string phone)
        {
            return await _decorated.FindByPhone(phone);
        }

    }
}
