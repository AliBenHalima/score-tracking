using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.CustomValidators
{
    public static class PasswordValidator
    {
        public static IRuleBuilder<T, string> IsPasswordValid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(HasUpperCase).WithMessage("Password must be at least has one Uppercase character")
                              .Must(HasLowerCase).WithMessage("Password must be at least has one lower character")
                              .Matches(@"\d").WithMessage("Password must be at least has one degit")
                              .Matches(@"[!@#$%^&]").WithMessage("Password must contain at least one special character (!, @, #, $, %, ^, &).");

        }

        private static bool HasUpperCase(string Password)
        {
            if (Password is null) return false;
            foreach (char c in Password)
            {
                if (char.IsUpper(c))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool HasLowerCase(string Password)
        {
            if (Password is null) return false;
            foreach (char c in Password)
            {
                if (char.IsLower(c))
                {
                    return true;
                }
            }
            return false;
        }
        //public static bool HasDegit(string? Password)
        //{
        //    if (Password is null) return false;
        //    return Regex.IsMatch(Password, @"\\d");

        //}

    }
}
