using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.CustomValidators
{
    public static class GameValidators 
    {
        public static IRuleBuilder<T, int?> CheckJokerOptionExists<T>(this IRuleBuilder<T, int?> ruleBuilder, bool hasJokerPenalty)
        {
            if (hasJokerPenalty)
            {
                return ruleBuilder.Must(x => x > 0);
            }
        return ruleBuilder.Must(x => x is null );
        }
    }
}
