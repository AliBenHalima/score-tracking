using FluentValidation;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.UserValidators
{
    public class CreateUserValidator: AbstractValidator<CreateUserRequest> 
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).Length(Constants.UserConstants.MinNameLength, Constants.UserConstants.MaxNameLength).NotEmpty();
            RuleFor(x => x.LastName).Length(Constants.UserConstants.MinNameLength, Constants.UserConstants.MaxNameLength).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone).Matches(Constants.UserConstants.PhoneRegex).WithMessage("Invalid phone number format.");
        }
    }
}
