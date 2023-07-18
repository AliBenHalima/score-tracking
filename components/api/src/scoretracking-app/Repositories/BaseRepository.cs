using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DatabaseContext databaseContext { get; set; }
        public BaseRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await databaseContext.Set<T>().AsNoTracking().ToListAsync();
        }
        public virtual async Task<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await databaseContext.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }
           
        public virtual async Task<T> Create(T entity)
        {
            databaseContext.Set<T>().Add(entity);
            await databaseContext.SaveChangesAsync();
            return entity;
           
        }
        public virtual async Task<T> Update(T entity)
        {
            databaseContext.Set<T>().Update(entity);
            await databaseContext.SaveChangesAsync();
            return entity;
        }
        public virtual async Task Delete(T entity) {
            databaseContext.Set<T>().Remove(entity);
            await databaseContext.SaveChangesAsync();
        }
    }
}
