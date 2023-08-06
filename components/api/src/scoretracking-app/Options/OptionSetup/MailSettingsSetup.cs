
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Options.OptionSetup
{
    public class MailSettingsSetup : IConfigureOptions<EmailSettings>
    {
        private readonly IConfiguration _configuration;

        public MailSettingsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(EmailSettings options)
        {
           _configuration.GetSection("MailSettings").Bind(options);
        }

    }
}
