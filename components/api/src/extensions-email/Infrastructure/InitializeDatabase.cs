using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Extensions.Options;
using Npgsql;
using Perfolizer.Horology;
using ScoreTracking.Extensions.Email.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Infrastructure
{
    public class InitializeDatabase
    {
        private readonly DatabaseOption _databaseOption;
        private static InitializeDatabase _instance;
        private static readonly object _lock = new object();

        public InitializeDatabase(IOptions<DatabaseOption> databaseOption)
        {
          
            _databaseOption = databaseOption.Value;
            CreateDatabase();
        }

        //public static InitializeDatabase GetInstance()
        //{
        //    if (_instance is null)
        //    {
        //        lock (_lock) // Prevents multiple threads for accessing this instance simultaniously
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new InitializeDatabase();
        //            }

        //        }
        //    }
        //    return _instance;
        //}

        private void CreateDatabase()
        {
            try
            {

                using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
                connection.Open();

                // Check if the table exists
                using (var command = new NpgsqlCommand(_databaseOption.CheckDatabaseExistanceQuery, connection))
                {
                    bool tableExists = (bool)command.ExecuteScalar();

                    if (!tableExists)
                    {
                        using var createCommand = new NpgsqlCommand(_databaseOption.CreateDatabaseQuery, connection);
                        createCommand.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Cant connect to database");
            }

        }






    }
}
