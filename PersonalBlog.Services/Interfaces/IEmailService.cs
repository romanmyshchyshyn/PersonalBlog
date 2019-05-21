using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailsAboutNewPostAsync(string title, string description, string postId);
    }
}
