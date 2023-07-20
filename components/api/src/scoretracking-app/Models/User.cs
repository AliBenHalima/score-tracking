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
    [Table("games")]
    public class Game : Base
    {
        public int Score { get; set; }
        public GameEndingType EndingType { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public bool HasJokerPenalty { get; set; }
        public int JokerPenaltyValue { get; set; }
        public IList<User> Users { get; set; }
        public IList<UserGame> UserGames { get; set; }


    }
}
