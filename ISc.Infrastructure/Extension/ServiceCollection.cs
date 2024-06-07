using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;
using ISc.Infrastructure.Services.Media;
using ISc.Application.Interfaces;
using ISc.Infrastructure.Services.Email;
using ISc.Infrastructure.Services.OnlineJudge.CodeForce;
using ISc.Infrastructure.Services.ApiRequest;
using ISc.Infrastructure.Services.Authentication;
using ISc.Infrastructure.Services.ScheduleTasks;
using Hangfire;

namespace ISc.Infrastructure.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddCollections()
                    .AddHangFireServices(configuration)
                    .AddFluentEmailServices(configuration)
                    .AddMemoryCache()
                    .AddDistributedMemoryCache()
                    .AddHangFireServices(configuration);
                   

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
                    .AddTransient<IEmailSender, EmailSender>()
                    .AddTransient<IAuthServices, AuthServices>()
                    .AddTransient<IEmailServices, EmailService>()
                    .AddTransient<IOnlineJudgeServices, CodeForceService>()
                    .AddTransient<IApiRequestsServices, ApiReqeustService>()
                    .AddTransient<IJobServices, JobService>(); ;

            return services;
        }

        private static IServiceCollection AddHangFireServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DataBase")))
                    .AddHangfireServer();

            return services;
        }

    }
}
