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
    public class UserGame
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public User User { get; set; } = null!;
    }
}
