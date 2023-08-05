using ScoreTracking.App.Enum;
using ScoreTracking.App.Models;
using ScoreTracking.App.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs
{
    public class RoundDTO
    {
        public int Id { get; set; }
        public int number { get; set; }
        public GameRoundStatus Status { get; set; } = GameRoundStatus.Played;
        public List<UserGameRoundDTO> UserGameRounds { get; } = new();

    }
}
