using Microsoft.Extensions.DependencyInjection;
using Quartz;
using ScoreTracking.App.Options.OptionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Extensions.BackgroundJobs
{
    public static class ElasticsearchExtension
    {
        public static void AddElasticsearchSynchronization(this IServiceCollection service)
        {
            service.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            service.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            service.ConfigureOptions<ElasticserchSynchronizationSetup>();
        }
    }
}
