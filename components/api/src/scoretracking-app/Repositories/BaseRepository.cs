using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        protected readonly DbSet<T> Entity;

        protected readonly DatabaseContext DatabaseContext;
        public BaseRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            Entity = databaseContext.Set<T>();
        }

        public virtual IQueryable<T> FindAll(FilterDTO filters, CancellationToken cancellationToken)
        {
            //var query = Entity.AsNoTracking();
            //return await PagedList<T>.CreateAsync(query, UriService, filters.Page, filters.PageSize, string.Empty);
            return Entity.AsNoTracking();

        }

        public virtual async Task<T?> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await Entity.Where(expression).FirstOrDefaultAsync();
        }

        public virtual async Task<T> FindById(int id)
        {
            return await Entity.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindByIds(IEnumerable<int> ids)
        {
            return await Entity.Where(u => ids.Any(x => x == u.Id)).ToListAsync();
        }

        public virtual Task<T> Create(T entity)
        {
            Entity.Add(entity);
            return Task.FromResult(entity);
        }

        public virtual Task<T> Update(T entity)
        {
            Entity.Update(entity);
            return Task.FromResult(entity);
        }

        public virtual Task Delete(T entity)
        {
            Entity.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
