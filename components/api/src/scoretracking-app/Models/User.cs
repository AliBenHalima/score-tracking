using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ScoreTracking.App.Models
{
    [Table("users")]
    public class User : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Password { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTimeOffset? PasswordChangedAt { get; set; }
        public DateTimeOffset? VerifiedAt { get; set; }
        [JsonIgnore]
        public List<Game> Games { get; set; } = new();
        [JsonIgnore]
        public List<UserGame> UserGames { get; set; } = new();
    }
}
