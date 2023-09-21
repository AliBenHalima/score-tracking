using Nest;
using System;

namespace ScoreTracking.App.DTOs.Users
{
    public record UserByDistanceDto : UserElasticsearchDto
    {
        public double? Distance { get; set; }

    }
}
