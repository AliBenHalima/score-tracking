using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScoreTracking.App.BackgroundJobs.Jobs;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;
using ScoreTracking.Extensions.Email.Infrastructure;
using ScoreTracking.Extensions.Email.Options;
using ScoreTracking.Extensions.Email.Services;

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


            services.AddSingleton<IEmailQueue, RabbitMqEmailQueue>();
            services.AddTransient<IEmailService, EmailService>();
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
