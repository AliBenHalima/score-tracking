using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Options
{
    public class DatabaseOptionSetup : IConfigureOptions<DatabaseOption>
    { 
        private readonly IConfiguration _configuration;

        public DatabaseOptionSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOption options)
        {
            _configuration.GetSection("DatabaseSetup").Bind(options);
        }
    }
    
}
