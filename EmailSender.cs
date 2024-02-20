using Microsoft.AspNetCore.Identity.UI.Services;

namespace BooksApp_Spring2024_sec01
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
