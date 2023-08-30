using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Database
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
        IDbTransaction BeginTransaction();

    }
}
