using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    [Table("user_game")]
    public class UserGame : Base
    {
        public int UserId{ get; set; }
        public int GameId { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }
    }
}
