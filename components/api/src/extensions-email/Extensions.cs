using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracking.App.BackgroundJobs.Jobs;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;
using ScoreTracking.Extensions.Email.Infrastructure;
using ScoreTracking.Extensions.Email.Options;
using ScoreTracking.Extensions.Email.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email
{
    public static class Extensions
    {
        public static IServiceCollection AddMailing(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var options = new EmailSettings();
                var config = sp.GetRequiredService<IConfiguration>();
                config.GetSection(EmailSettings.SectionName).Bind(options);
                return options;
            });


            services.AddSingleton<IEmailQueue, DatabaseEmailQueue>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IEmailSender, MailTrapSender>();
            services.AddSingleton<IEmailSender, FakeEmailSender>();
            services.AddHostedService<BackgroundEmailWorker>();
            services.AddAutoMapper(typeof(IModuleMarker).Assembly);
            services.AddSingleton<InitializeDatabase>();
            services.ConfigureOptions<DatabaseOptionSetup>();
            services.AddSingleton<IEmailRepository, PgEmailRepository>();
            return services;
        }
    }
}
