using ScoreTracking.App.DTOs.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Queues
{
    public interface IEmailQueue
    {
        void EnQueueEmailTask(EmailDataDTO emailData);
        EmailDataDTO DeQueueEmailTask();
    }
}
