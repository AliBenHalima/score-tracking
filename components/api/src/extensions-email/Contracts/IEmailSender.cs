using ScoreTracking.Extensions.Email.Contratcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Contracts
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
