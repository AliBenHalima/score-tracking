using ScoreTracking.App.DTOs.Emails;
using ScoreTracking.App.Interfaces.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.BackgroundJobs.Queues
{
    public class InMemoryEmailQueue : IEmailQueue
    {

        private readonly Queue<EmailDataDTO> _emailQueue = new Queue<EmailDataDTO>();

        public void EnQueueEmailTask(EmailDataDTO emailData)
        {
            _emailQueue.Enqueue(emailData);
        }

        public EmailDataDTO? DeQueueEmailTask()
        {
            if (_emailQueue.Count() > 0)
            {
                return _emailQueue.Dequeue();
            }
            return null;
        }

    }
}
