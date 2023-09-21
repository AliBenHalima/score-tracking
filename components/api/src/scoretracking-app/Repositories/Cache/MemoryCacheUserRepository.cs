using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Extensions.Query;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories.Cache
{
    public class MemoryCacheUserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheUserRepository(DatabaseContext databaseContext, UserRepository userRepository, IMemoryCache memoryCache) : base(databaseContext)
        {
            _decorated = userRepository;
            _memoryCache = memoryCache;
        }

        public override async Task<PagedList<User>?> FindAll(FilterDTO filters, CancellationToken cancellationToken)
        {
            string key = "users";
            PagedList<User>? query = await _memoryCache.GetOrCreate(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return _decorated.FindAll(filters, cancellationToken);
            });

            return query;
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

        public Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
