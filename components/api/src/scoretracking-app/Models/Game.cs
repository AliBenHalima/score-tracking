using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    [Table("games")]
    public class Game : Base
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Score { get; set; }
        public GameEndingType? EndingType { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public bool HasJokerPenalty { get; set; }
        public int JokerPenaltyValue { get; set; }
        public List<User> Users { get; } = new();
        public List<UserGame> UserGames { get; } = new();



    }
}
