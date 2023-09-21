using Microsoft.Extensions.Options;
using Quartz;
using ScoreTracking.App.Jobs;
using System;

namespace ScoreTracking.App.Options.OptionSetup
{
    public class ElasticserchSynchronizationSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(ElasticsearchSyncJob));
            options.AddJob<ElasticsearchSyncJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                     .AddTrigger(trigger =>
                         trigger.ForJob(jobKey)
                                .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(2).RepeatForever()));
        }
    }
}
