using Nest;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracking.App.Elasticsearch
{
    public interface ISearchClient
    {
        Task<ISearchResponse<UserElasticsearchDto>> Search(string searchText);
        Task<IEnumerable<UserElasticsearchDto>> GetAll();
        Task Add();
        Task BulkAdd(IEnumerable<UserElasticsearchDto> data);
        Task BulkUpdate(IEnumerable<UserElasticsearchDto> data);
        Task BulkDelete(IEnumerable<UserElasticsearchDto> data);
        IReadOnlyCollection<IHit<UserElasticsearchDto>> GetUserByDistance(int distance, double logitude, double latitude);
    }
}
