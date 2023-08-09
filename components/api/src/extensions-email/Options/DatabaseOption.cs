using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Options
{
    public class DatabaseOption
    { 
            public string ConnectionString { get; init; }
            public string CreateDatabaseQuery { get; init; }
            public string CheckDatabaseExistanceQuery { get; init; }
        }
    
}
