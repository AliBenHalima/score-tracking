using Quartz.Impl;
using Quartz;
using ScoreTracking.Extensions.Email.Contratcs;

namespace ScoreTracking.Extensions.Email.Services
{
    public class QuartzEmailService : IEmailService
    {
        public async Task SendAsync(EmailMessage email, CancellationToken cancellationToken = default)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();
            JobDataMap m = new JobDataMap();
            m.Put("email", email);

            IJobDetail job = JobBuilder.Create<Jobs.TestJob>()
            .WithIdentity(DateTime.UtcNow.ToString())
            .UsingJobData(m)
            .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(DateTime.UtcNow.ToString())
              .StartNow()
              .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
