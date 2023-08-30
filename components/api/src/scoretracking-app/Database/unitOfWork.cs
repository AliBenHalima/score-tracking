using Microsoft.EntityFrameworkCore.Storage;
using ScoreTracking.App.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUserRepository _userRepository;


        private bool _disposingValue = false;

        public UnitOfWork(DatabaseContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _dbContext.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposingValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposingValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
