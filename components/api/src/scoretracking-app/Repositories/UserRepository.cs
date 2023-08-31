﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Extensions.Query;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    [SimpleJob(RuntimeMoniker.Net70)]
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override IQueryable<User> FindAll(FilterDTO filters, CancellationToken cancellationToken)
        {
            IQueryable<User> userQuery = Entity;
            userQuery = userQuery.SearchByTerm(filters.SearchTerm)
                        .OrderBy(u => u.FirstName)
                        .ApplySorting(filters.SortColumn, filters.SortOrder);
            return userQuery;
        }

        public override Task<User> Create(User user)
        {
            DatabaseContext.Users.Add(user);
             return Task.FromResult(user);
        }

        [Benchmark]
        [Arguments("AliBeHalima61@gmail.com")]
        public async Task<User?> FindByEmail(string email)
        {
            return await Entity.Where(u => u.Email == email).AsNoTracking().FirstOrDefaultAsync();
        }

        [Benchmark]
        [Arguments("+216540142572")]

        public async Task<User?> FindByPhone(string phone)
        {
            return await Entity.Where(u => u.Phone == phone).AsNoTracking().FirstOrDefaultAsync();
        }

    }
}
