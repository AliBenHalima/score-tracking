using Quartz;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;

namespace ScoreTracking.Extensions.Email.Jobs
{
    public class TestJob : IJob
    {
        private readonly IEmailSender _emailSender;

        public TestJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            EmailMessage data = (EmailMessage)dataMap.Get("email");

            await _emailSender.Send(data);
        }
    }
}
