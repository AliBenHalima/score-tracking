using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    [Table("rounds")]
    public class Round : Base
    {
        public int number { get; set; }
        public GameRoundStatus Status { get; set; } = GameRoundStatus.Played;
        public List<UserGame> UserGames { get; } = new();
        public List<UserGameRound> UserGameRounds { get; } = new();
    }
}
