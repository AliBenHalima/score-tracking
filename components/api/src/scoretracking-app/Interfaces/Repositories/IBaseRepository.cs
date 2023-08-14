using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : Base
    {
        IQueryable<T> FindAll(FilterDTO filters, CancellationToken cancellationToken);
        Task<T> FindById(int id);
        Task<IEnumerable<T>> FindByIds(IEnumerable<int> ids);
        Task<T?> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
    }

}
