using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Extensions.Query;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories.Cache
{
    public class CacheUserRepository: BaseRepository<User>, IUserRepository
    {
        private readonly UserRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public CacheUserRepository(DatabaseContext databaseContext, UserRepository userRepository, IMemoryCache memoryCache) : base(databaseContext)
        {
            _decorated = userRepository;
            _memoryCache = memoryCache;
        }

        public override IQueryable<User> FindAll(FilterDTO filters, CancellationToken cancellationToken)
        {
            string key = "users";
            _memoryCache.GetOrCreate(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return _decorated.FindAll(filters, cancellationToken);
            });

            IQueryable<User> userQuery = Entity;
            userQuery = userQuery.SearchByTerm(filters.SearchTerm)
                        .OrderBy(u => u.FirstName)
                        .ApplySorting(filters.SortColumn, filters.SortOrder);
            return userQuery;
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
