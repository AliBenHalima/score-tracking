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
    public class CreateGameValidation : AbstractValidator<CreateGameRequest> 
    {
        public CreateGameValidation()
        {
            RuleFor(x => x.Name).Length(Constants.GameConstants.MinNameLength, Constants.GameConstants.MaxNameLength).NotEmpty();
            RuleFor(x => x.Score).GreaterThanOrEqualTo(Constants.GameConstants.MinimumScore).NotEmpty();
            //TODO, Add validation about property type(must be a boolean)
            RuleFor(x => x.HasJokerPenalty).NotNull().WithMessage("Value must not be empty");
            // TODO, Add validation for UserIds since they throw error when string values inserted
            When(x => x.HasJokerPenalty is true, () =>
            {
                RuleFor(x => x.JokerPenaltyValue).Must(x => x > 0).WithMessage("Joker value must be greater than 0 , You entered the value {PropertyValue}");
            });
            RuleFor(x => x.UserIds).Must(CheckDublicated).WithMessage("PlayerIds must not contain duplicated elements")
                                   .Must(CheckCount).WithMessage("Added players can't overpass 3 players, You entered {PropertyValue} players");
            //RuleFor(x => x.JokerPenaltyValue).CheckJokerOptionExists();

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
