using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScoreTracking.App.Database;
using System;

namespace ScoreTracking.UnitTests
{
    public class InMemoryDatabaseContext
    {
        public DatabaseContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "in-memory-database").Options;

            var context = new DatabaseContext(option);
            if (context is not null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}

