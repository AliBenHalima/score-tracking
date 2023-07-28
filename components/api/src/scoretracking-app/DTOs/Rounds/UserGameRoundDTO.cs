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
    public class UserGameRoundDTO
    {
        public int Id { get; set; }
        public int UserGameId { get; set; }
        public int RoundId { get; set; }
        public int Jokers { get; set; }
        public UserGame UserGame { get; set; } = null!;

    }
}
