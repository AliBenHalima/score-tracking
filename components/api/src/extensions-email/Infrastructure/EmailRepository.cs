using Microsoft.Extensions.Options;
using Npgsql;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Options;


namespace ScoreTracking.Extensions.Email.Contratcs
{
    public class PgEmailRepository : IEmailRepository
    {
        private readonly DatabaseOption _databaseOption;

        public PgEmailRepository(IOptions<DatabaseOption> databaseOption)
        {
            _databaseOption = databaseOption.Value;
        }
        public async Task<int?> InsertIntoEmailQueueAsync(EmailMessage email)
        {
            using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
            connection.Open();

            using var insertCommand = new NpgsqlCommand(@"INSERT INTO email_queue (ReceiverName, ReceiverAddress, Content, Subject) VALUES (@ReceiverName, @ReceiverAddress, @Content, @Subject) RETURNING Id", connection);
            insertCommand.Parameters.AddWithValue("@ReceiverName", email.ReceiverName);
            insertCommand.Parameters.AddWithValue("@ReceiverAddress", email.ReceiverAddress);
            insertCommand.Parameters.AddWithValue("@Content", email.Content);
            insertCommand.Parameters.AddWithValue("@Subject", email.Subject);
            int? insertedId = (int)await insertCommand.ExecuteScalarAsync();
            return insertedId;

        }

        public async Task MarkEmailQueueAsProcessedAsync(string id)
        {
            using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
            connection.Open();
            using var insertCommand = new NpgsqlCommand(@"UPDATE email_queue SET IsProcessed = true WHERE Id = @Id", connection);

            insertCommand.Parameters.AddWithValue("@Id", Int32.Parse(id));
            var rowsAffected = await insertCommand.ExecuteNonQueryAsync();

        }

        public async Task<EmailQueueEntity> RemoveLatestItemAsync()
        {

            try
            {
                EmailQueueEntity message = default;
                using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
                connection.Open();

                using var selectCommand = new NpgsqlCommand(@"SELECT * FROM email_queue WHERE IsProcessed = false ORDER BY CreatedAt ASC LIMIT 1", connection);

                using var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    message = new EmailQueueEntity
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")).ToString(),
                        ReceiverName = reader.GetString(reader.GetOrdinal("ReceiverName")),
                        ReceiverAddress = reader.GetString(reader.GetOrdinal("ReceiverAddress")),
                        Content = reader.GetString(reader.GetOrdinal("Content")),
                        Subject = reader.GetString(reader.GetOrdinal("Subject")),
                        IsProcessed = reader.GetBoolean(reader.GetOrdinal("IsProcessed")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                }
                return message;
            }

            catch (Exception ex)
            {
                throw new Exception("Can't send email");
            }
        }
        public async Task<bool> DatabaseHasItemAsync()
        {
            using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
            connection.Open();
            using (var selectCommand = new NpgsqlCommand(@"SELECT COUNT(*) FROM email_queue WHERE IsProcessed = false", connection))
            {
                int rowCount = Convert.ToInt32(await selectCommand.ExecuteScalarAsync());

                return rowCount > 0 ? true : false;

            }
        }
        public async Task MarkEmailQueueAsSucceededAsync(string id)
        {
            using var connection = new NpgsqlConnection(_databaseOption.ConnectionString);
            connection.Open();
            using var insertCommand = new NpgsqlCommand(@"UPDATE email_queue SET IsSuccessful = true WHERE Id = @Id", connection);

            insertCommand.Parameters.AddWithValue("@Id", Int32.Parse(id));
            var rowsAffected = await insertCommand.ExecuteNonQueryAsync();
        }
    }
}
