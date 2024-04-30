using System.Xml.Linq;
using ISc.Application.Dtos.Email;
using ISc.Application.Interfaces;
using ISc.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ISc.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailServices _emailService;
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _filePath;
        private readonly string _email;

        public EmailSender(
            IConfiguration config,
            IEmailServices emailService,
            IWebHostEnvironment host)
        {
            _filePath = config.GetSection("EmailTemplates");
            _email = config.GetSection("MailSettings:SenderEmail").Value!;
            _emailService = emailService;
            _host = host;
        }

        public async Task<bool> SendAcceptTraineeEmailAsync(Trainee trainee, string campName, DateOnly startDate)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["AcceptedTrainee"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = trainee.Email!,
                From = _email,
                Subject = "ISc - Welcome in ICPC Sohag Community",
                Body = content,
                BodyData = new
                {
                    CampName = campName,
                    TraineeName = trainee.FirstName + ' ' + trainee.LastName,
                    StartDate = startDate
                }
            });
        }

        public async Task<bool> SendAccountInfoAsync(string userName, string password, Trainee trainee)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["AccoutInfo"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = trainee.Email!,
                From = _email,
                Subject = "ISc - Account Information",
                Body = content,
                BodyData = new
                {
                    TraineeName = trainee.FirstName + ' ' + trainee.LastName,
                    UserName = userName,
                    Password = password
                }
            });
        }

        public async Task<bool> SendEmailConfirmationAsync(string email, int otp)
        {
            var x = _filePath["EmailConfirmation"];
            var content = File.ReadAllText(_host.WebRootPath + _filePath["EmailConfirmation"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "ISc - Email Confirmation Otp",
                Body = content,
                BodyData = new
                {
                    Otp = otp
                }
            });
        }

        public async Task<bool> SendForgetPassword(string email, string name, int otp)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["OtpResetPassword"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "ISc - Forget Password Otp",
                Body = content,
                BodyData = new
                {
                    Name = name,
                    Otp = otp
                }
            });
        }

        public async Task<bool> SendKickedoutEmailAsync(string email, string campName, string traineeName)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["Kickedout"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "Isc - Filteration System Annoucment",
                Body = content,
                BodyData = new
                {
                    CampName = campName,
                    TraineeName = traineeName
                }
            });
        }

        public async Task<bool> SendRejectionEmailAsync(string email, string applicantName)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["Rejection"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "ISc - Join request Annoucment",
                Body = content,
                BodyData = new
                {
                    ApplicantName = applicantName
                }
            });
        }
    }
}
