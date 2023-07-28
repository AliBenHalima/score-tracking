using FluentValidation;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Models;
using ScoreTracking.App.Validations.CustomValidators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.UserValidators
{
    public class AddPlayersToGameValidation : AbstractValidator<AddPlayersToGameRequest> 
    {
        public AddPlayersToGameValidation()
        {
            RuleFor(x => x.PlayerIds).Must(CheckDublicated).WithMessage("PlayerIds must not contain duplicated elements")
                                     .Must(CheckCount).WithMessage("Added players can't overpass 3 players");
        }

        private bool CheckDublicated(IEnumerable<int> values)
        {
            return values.Distinct().Count() == values.Count();
        }
        private bool CheckCount(IEnumerable<int> values)
        {
            return values.Distinct().Count() <= Constants.GameConstants.MaxAddedPlayersCount;
        }
    }
}
