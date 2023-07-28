using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs
{
    public class RoundInformationDTO
    {
        public int UserId { get; set; }
        public int Score { get; set; }
        public int JokerCount { get; set; }

    }
}
