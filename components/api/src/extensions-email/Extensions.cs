using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using Quartz;
using MassTransit;
using Microsoft.Extensions.Options;
using ScoreTracking.Extensions.Email.Services.Shared;
using ScoreTracking.Extensions.Email.Contracts.Shared;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ScoreTracking.Extensions.Email
{
    public static class Extensions
    {
        public static async Task<IServiceCollection> AddMailing(this IServiceCollection services)
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

            services.ConfigureOptions<MessageBrokerOptionSetup>();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerOption>>().Value);
            services.AddTransient<IEventBus, RabbitMqMassTransitProducer>();

            services.AddMassTransit(busConfigurator=>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumer<RabbitMqMassTransitConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerOption settings = context.GetRequiredService<MessageBrokerOption>();
                    configurator.Host(new Uri(settings.Host), conf =>
                    {
                        conf.Username(settings.Username);
                        conf.Password(settings.Password);
                    });
                    configurator.ReceiveEndpoint("email-message-sent-event", e =>
                    {
                        e.ConfigureConsumer<RabbitMqMassTransitConsumer>(context); // Configure the consumer for this endpoint
                    });
                });
            });

            return services;
        }
    }
}
