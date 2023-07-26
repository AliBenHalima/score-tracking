using FluentValidation;
using ScoreTracking.App.DTOs.Requests;
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
            RuleFor(x => x.FirstName).Length(2, 100).NotEmpty();
            RuleFor(x => x.LastName).Length(2, 100).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone).Matches(@"^\+\d{1,3}\s?\d{6,14}$").WithMessage("Invalid phone number format.");
        }
    }
}
