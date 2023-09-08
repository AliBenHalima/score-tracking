using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Options
{
    public class MessageBrokerOptionSetup : IConfigureOptions<MessageBrokerOption>
    {
        private readonly IConfiguration _configuration;

        public MessageBrokerOptionSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MessageBrokerOption options)
        {
            _configuration.GetSection("MessageBrokerSetup").Bind(options);
        }
    }
}
