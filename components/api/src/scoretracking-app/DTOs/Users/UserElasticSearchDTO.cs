using Nest;
using System;

namespace ScoreTracking.App.DTOs.Users
{
    public record UserElasticsearchDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [GeoPoint]
        public GeoLocation Location { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
