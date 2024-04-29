using FluentEmail.Core;
using ISc.Application.Dtos.Email;
using ISc.Application.Interfaces;
using ISc.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ISc.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly IEmailServices _emailService;
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _filePath;
        private readonly string _email;

        public EmailSender(
            IConfiguration config,
            IEmailServices emailService,
            IWebHostEnvironment host)
        {
            _config = config;
            _filePath = config.GetSection("MailSettings");
            _email = _config.GetSection("MailSettings:SenderEmail").Value!;
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
                Subject = "Welcome in ICPC Sohag Community",
                Body = content,
                BodyData = new
                {
                    CampName = campName,
                    TraineeName = trainee.FirstName + ' ' + trainee.LastName,
                    StartDate = startDate
                }
            });
        }

        public async Task<bool> SendAccountInfoAsync(string userName, string Password,Trainee trainee)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["AccoutInfo"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = trainee.Email!,
                From = _email,
                Subject = "ISc Account Information",
                Body = content,
                BodyData = new
                {
                    TraineeName = trainee.FirstName+' '+trainee.LastName,
                    UserName = userName,
                    Password
                }
            });
        }

        public async Task<bool> SendForgetPassword(string email,string Name,int Otp)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["OtpResetPassword"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "ISc Forget Password Otp",
                Body = content,
                BodyData = new
                {
                    Name,
                    Otp
                }
            });
        }

        public async Task<bool> SendKickedoutEmailAsync(string email,string CampName,string TraineeName)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["OtpResetPassword"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "",
                Body = content,
                BodyData = new
                {
                    CampName,
                    TraineeName
                }
            });
        }

        public async Task<bool> SendRejectionEmailAsync(string email, string ApplicantName)
        {
            var content = File.ReadAllText(_host.WebRootPath + _filePath["OtpResetPassword"]);

            return await _emailService.SendMailUsingRazorTemplateAsync(new EmailRequestDto()
            {
                To = email!,
                From = _email,
                Subject = "",
                Body = content,
                BodyData = new
                {
                    ApplicantName
                }
            });
        }
    }
}
