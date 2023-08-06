using Org.BouncyCastle.Asn1.Pkcs;
using ScoreTracking.App.DTOs.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IEmailService {
        bool SendEmail(EmailDataDTO emailData);
        Task<bool> SendEmailAsync(EmailDataDTO emailData);
        Task<bool> SendHTMLEmailAsync(EmailDataDTO htmlEmailData);

    }
}
