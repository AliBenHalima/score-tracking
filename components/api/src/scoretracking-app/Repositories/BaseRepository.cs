using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await Entity.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<T> FindById(int id)
        {
            return await Entity.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindByIds(IEnumerable<int> ids)
        {
            return await Entity.Where(u => ids.Any(x => x == u.Id)).ToListAsync();
        }

        public virtual async Task<T> Create(T entity)
        {
            Entity.Add(entity);
            await DatabaseContext.SaveChangesAsync();
            return entity;

        }

        public virtual async Task<T> Update(T entity)
        {
            Entity.Update(entity);
            await DatabaseContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(T entity)
        {
            Entity.Remove(entity);
            await DatabaseContext.SaveChangesAsync();
        }
    }
}
