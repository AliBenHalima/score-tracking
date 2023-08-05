using DataAnnotationsExtensions;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Validations;
using ScoreTracking.App.Validations.UserValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Requests.Games
{
    public class CreateGameRequest
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool HasJokerPenalty { get; set; }
        public int? JokerPenaltyValue { get; set; }
        public IEnumerable<int> UserIds { get; set; }
    }
}
