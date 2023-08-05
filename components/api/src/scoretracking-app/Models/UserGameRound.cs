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
    [Table("user_game_rounds")]
    public class UserGameRound : Base
    {
        public int UserGameId{ get; set; }
        public int RoundId { get; set; }
        public int Jokers { get; set; }
        public int Score { get; set; }
        public UserGame UserGame { get; set; } = null!;
        public Round Round { get; set; } = null!;
    }
}
