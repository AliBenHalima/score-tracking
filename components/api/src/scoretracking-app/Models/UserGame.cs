using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    [Table("user_games")]
    public class UserGame : Base
    {
        public int UserId{ get; set; }
        public int GameId { get; set; }
        public User User { get; set; } = null!;
        public Game Game { get; set; } = null!;
        [JsonIgnore]
        public List<Round> Rounds { get; } = new();
        [JsonIgnore]
        public List<UserGameRound> UserGameRounds { get; } = new();
    }
}
