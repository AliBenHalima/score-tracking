using FluentValidation;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.DTOs.Requests.Rounds;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Validations.CustomValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.AuthenticationValidators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.FirstName).Length(Constants.UserConstants.MinNameLength, Constants.UserConstants.MaxNameLength).NotEmpty();
            RuleFor(x => x.LastName).Length(Constants.UserConstants.MinNameLength, Constants.UserConstants.MaxNameLength).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone).Matches(Constants.UserConstants.PhoneRegex).WithMessage("Invalid phone number format.");
            RuleFor(x => x.Password).IsPasswordValid();
            RuleFor(x => x.ConfirmPassword)
                    .NotEmpty()
                    .WithMessage("Confirm Password is required.")
                    .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
