using ScoreTracking.App.Interfaces.Helpers;
using System;


namespace ScoreTracking.App.Helpers
{
    public class GlobalHelper : IApplicationHelper
    {
        public  bool AreIntegersEqual(int? firstValue, int? secondValue)
        {
            return firstValue == secondValue;
        }

        public  string generateRandom(int min = 1000, int max= 9999)
        {
            Random random = new Random();
            return random.Next(min, max).ToString();
        }
    }
}
