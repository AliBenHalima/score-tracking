using FluentValidation;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Rounds;
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
    public class AddRoundValidator : AbstractValidator<AddRoundRequest> 
    {
        public AddRoundValidator()
        {
            RuleFor(x => x.RoundInformation).ForEach(r => r.SetValidator(new RoundInformationCustomValidator()));        
        }

    }
}
