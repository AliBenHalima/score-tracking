using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class GameDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Score { get; set; }
        public GameEndingType? EndingType { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public DateTimeOffset? CanceledAt { get; set; }
        public bool HasJokerPenalty { get; set; }
        public int JokerPenaltyValue { get; set; }
        public List<Round> Rounds { get; set; }
        public List<RoundSumDTO> RoundSum { get; set; }
    }
}
