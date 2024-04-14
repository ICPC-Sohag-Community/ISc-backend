using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ISc.Infrastructure.Services.Media;
using ISc.Application.Interfaces;
using ISc.Infrastructure.Services.Email;
using ISc.Infrastructure.Services.OnlineJudge.CodeForce;

namespace ISc.Infrastructure.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddCollections()
                    .AddHangFireServices(configuration)
                    .AddFluentEmailServices(configuration);
                   

            return services;
        }

        private static IServiceCollection AddFluentEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("MailSettings");
            var defaultFromEmail = emailSettings["SenderEmail"];
            var host = emailSettings["Server"];
            var port = emailSettings.GetValue<int>("Port");
            var userName = emailSettings["UserName"];
            var password = emailSettings["Password"];
            var enableSsl = emailSettings.GetValue<bool>("EnableSSL");
            var useDefaultCredentials = emailSettings.GetValue<bool>("UseDefaultCredentials");

            var smtpClient = new SmtpClient
            {
                EnableSsl = enableSsl,
                Host = host,
                Port = port,
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = new NetworkCredential(userName, password),
            };

            services.AddFluentEmail(defaultFromEmail)
                    .AddRazorRenderer()
                    .AddLiquidRenderer()
                    .AddSmtpSender(smtpClient);

            return services;
        }
        private static IServiceCollection AddCollections(this IServiceCollection services)
        {
            services.AddTransient<IMediaServices, MediaServices>()
                    .AddTransient<IEmailSender,EmailSender>()
                    .AddTransient<IEmailServices,EmailService>()
                    .AddTransient<IOnlineJudgeServices,CodeForceService>();

            return services;
        }

        private static IServiceCollection AddHangFireServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")))
                    .AddHangfireServer();

            return services;
        }

    }
}
