using FluentValidation;
using ScoreTracking.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.CustomValidators
{
    public  class RoundInformationCustomValidator : AbstractValidator<RoundInformationDTO>
    {
        public RoundInformationCustomValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Score).NotNull();
            RuleFor(x => x.JokerCount).NotNull();
        }
    }
}
