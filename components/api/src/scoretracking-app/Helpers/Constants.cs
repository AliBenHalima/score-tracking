using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Helpers
{
    public class Constants
    {
        public class UserConstants
        {
            public const int MinimumScore = 700;
            public const int MinNameLength = 2;
            public const int MaxNameLength = 100;
            public const string PhoneRegex = @"^\+\d{1,3}\s?\d{6,14}$";

        }
        public class GameConstants
        {
            public const int MinimumScore = 700;
            public const int MinNameLength = 2;
            public const int MaxNameLength = 50;
            public const int MaxPlayersCount =4; // Max players per game 
            public const int MaxAddedPlayersCount = 3; // Max players to add per game
        }
    }
}
