using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using ScoreTracking.App.DTOs.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Elasticsearch
{
    public class ElasticsearchWorker : BackgroundService
    {
        private readonly IElasticClient _client;
        private readonly ILogger<ElasticsearchWorker> _logger;
        public ElasticsearchWorker(IElasticClient client, ILogger<ElasticsearchWorker> logger)
        {
            _client = client;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var data = new RegistredUserDTO
            {
                FirstName = "Ali",
                LastName = "BH",
                Email = "Ali@gmail.com",
                Phone = "+21658785874",
            };
            var result = await _client.IndexDocumentAsync(data);
            _logger.LogInformation($" Elasticsearch response {result.Id}");
        }
    }
}
