using Elasticsearch.Net;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Nest;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreTracking.App.Elasticsearch
{
    public class UserSearch : ISearchClient
    {
        private readonly ElasticClient _client;

        public UserSearch (IConfiguration configuration)
        {
            var settings = new ConnectionSettings(configuration.GetValue<string>("Elastic:cloudId"), new BasicAuthenticationCredentials(configuration.GetValue<string>("Elastic:username"), configuration.GetValue<string>("Elastic:password")))
                               .DefaultIndex("users-index");
            //.DefaultMappingFor<UserElasticsearchDto>(i => i.IndexName("users-index")); // For default mapping
            _client = new ElasticClient(settings);
            _client.Indices.Create("users-index", c => c
            .Map<UserElasticsearchDto>(m => m
            .AutoMap() // Automatically map other properties
            .Properties(p => p
            .GeoPoint(g => g
            .Name(n => n.Location) // Map the "Location" property as a geo_point
                )
            )
            )
            );
        }

        public async Task<IEnumerable<UserElasticsearchDto>> GetAll ()
        {
            var scrollTimeout = "5m";

            // Initial search request to start the scroll
            var searchResponse = await _client.SearchAsync<UserElasticsearchDto>(s => s
                .Query(q => q.MatchAll())
                .Size(100)
                .Scroll(scrollTimeout) // Set the scroll timeout
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception("Error with fetching data");
            }

            var allDocuments = new List<UserElasticsearchDto>();
            var scrollId = searchResponse.ScrollId;

            while (searchResponse.Documents.Any())
            {
                allDocuments.AddRange(searchResponse.Documents);

                // Continue scrolling
                var scrollRequest = new ScrollRequest(scrollId, scrollTimeout);
                searchResponse = await _client.ScrollAsync<UserElasticsearchDto>(scrollRequest);

                if (!searchResponse.IsValid)
                {
                    throw new Exception("Error with fetching data");
                }
            }

            // Clear the scroll
            await _client.ClearScrollAsync(c => c.ScrollId(scrollId));
            return allDocuments;
        }


        public async Task<ISearchResponse<UserElasticsearchDto>> Search (string searchText)
        {
            return await _client.SearchAsync<UserElasticsearchDto>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.FirstName)
                        .Query(searchText)
                    )
                ));
        }

        public async Task Add ()
        {
            var data = new UserElasticsearchDto
            {
                FirstName = "Ali",
                LastName = "BH",
                Email = "Ali@gmail.com",
                Phone = "+21658785874",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            var result = await _client.IndexDocumentAsync(data);
        }

        public async Task BulkAdd (IEnumerable<UserElasticsearchDto> data)
        {
            _client.Bulk(b => b
                    .Index("users-index")
                    .CreateMany(data));
        }

        public async Task BulkUpdate (IEnumerable<UserElasticsearchDto> data)
        {
            await _client.BulkAsync(b => b
                           .UpdateMany<UserElasticsearchDto>(data, (descriptor, user) => descriptor
                           .Id(user.Id) // Specify the _id of the document to update
                           .Doc(user)
                           .RetriesOnConflict(3) // used for Opimistic concurrency control OCC
                           ));
        }
        public async Task BulkDelete (IEnumerable<UserElasticsearchDto> data)
        {
            await _client.BulkAsync(b => b
           .DeleteMany<UserElasticsearchDto>(data, (descriptor, user) => descriptor
           .Index("users-index")
           .Id(user.Id))); // the document ID to delete

        }
        public IReadOnlyCollection<IHit<UserElasticsearchDto>> GetUserByDistance (int distance, double latitude, double logitude)
        {
            var location = new GeoLocation(latitude, logitude);
            var searchResponse = _client.Search<UserElasticsearchDto>(s => s
        .Query(q => q
          .Bool(b => b
              .Filter(filter => filter
                  .GeoDistance(geo => geo
                      .Field(f => f.Location)
                      .Distance(distance + "km")
                      .Location(location) // Provide the reference location
                  )
              )
          )
      )
        .Sort(sort => sort
            .GeoDistance(g => g
            .Field(f => f.Location) // Field containing user locations
            .Order(SortOrder.Ascending) // Ascending order (nearest users first)
            .Unit(DistanceUnit.Kilometers) // Specify the unit of distance
            .Points(location) // Reference location for distance calculation
        )
    )
      .Source(src => src
        .Includes(i => i
            .Field(f => f.Id)
            .Field(f => f.FirstName)
            .Field(f => f.LastName)
            .Field(f => f.Email)
            .Field(f => f.Phone)
            .Field(f => f.Location)
            .Field(f => f.CreatedAt)
            .Field(f => f.UpdatedAt)
        ))
       .Size(100)
  );
            return searchResponse.Hits;
        }

    }
}
