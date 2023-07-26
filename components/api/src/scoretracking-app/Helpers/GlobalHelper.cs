using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Helpers
{
    public static class GlobalHelper
    {
        public static bool AreIntegersEqual(int? firstValue, int? secondValue)
        {
            return firstValue == secondValue;
        }
    }
}
