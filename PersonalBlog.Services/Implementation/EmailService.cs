using MimeKit;
using MailKit.Net.Smtp;
using PersonalBlog.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using PersonalBlog.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Linq;
using PersonalBlog.DataAccess.Models;

namespace PersonalBlog.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public EmailService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task SendEmailsAboutNewPostAsync(string title, string description, string postId)
        {
            var angularAppUrl = _configuration["AngularAppUrl"];
            var message = String.Join("", _configuration.GetSection("EmailConfiguration:NewPostTemplate").Get<List<string>>());
            message = message
                .Replace("#TITLE", title)
                .Replace("#DESCRIPTION", description)
                .Replace("#URL", $"{angularAppUrl}/blog/{postId}");

            var appEmail = _configuration["EmailConfiguration:Email"];
            var appEmailPassword = _configuration["EmailConfiguration:Password"];
            var name = _configuration["EmailConfiguration:Name"];
            var subject = _configuration["EmailConfiguration:NewPostSubject"];
            var smtpServer = _configuration["EmailConfiguration:GmailSmtpServer"];
            var port = int.Parse(_configuration["EmailConfiguration:GmailPort"]);

            var users = _unitOfWork.GetRepository<User>()
                .Get(u => u.IsSubscribed)
                .Select(u => new MailboxAddress("", u.Email))
                .ToList();

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(name, appEmail));
            emailMessage.To.AddRange(users);
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, port, true);
                await client.AuthenticateAsync(appEmail, appEmailPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
