using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Helpers
{
    public interface IApplicationHelper
    {
        bool AreIntegersEqual(int? firstValue, int? secondValue);
        string generateRandom(int min, int max);
    }
}
