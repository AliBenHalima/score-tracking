using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.UserValidators
{
    public class RequiredIfPenaltyExists: ValidationAttribute
    {
        //private readonly string _dependentPropertyName;

        //public RequiredIfPenaltyExists(string dependentPropertyName)
        //{
        //    _dependentPropertyName = dependentPropertyName;
        //}

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var dependentPropertyValue = validationContext.ObjectType.GetProperty(_dependentPropertyName)?.GetValue(validationContext.ObjectInstance, null);

        //    if (dependentPropertyValue != null && dependentPropertyValue is true)
        //    {
        //        if (value is not int)
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }

        //    return ValidationResult.Success;
        //}
    }
}
