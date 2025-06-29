using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using CustomerSupport.Interfaces;

namespace CustomerSupport.Services;

public class EmailService : IEmailService
{
    public bool SendEmail(string email, string token)
    {
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("srisathyasai24680@gmail.com", "xhjv kaxv uagf oxxs"),
            EnableSsl = true
        };

        MailMessage message = new MailMessage
        {
            From = new MailAddress("srisathyasai24680@gmail.com"),
            Subject = "Reset-Password Link",
            IsBodyHtml = true
        };

        message.To.Add(email);

        StringBuilder mailBody = new StringBuilder();
        mailBody.Append("<h1> Reset Password Link: </h1>");
        mailBody.Append("<p>As per your request, please find the link below to reset the password:</p>");
        mailBody.Append($"<p><a href='http://localhost:4200/resetPassword?email={email}&token={token}' target='_blank'>Click here to reset password</a></p>");
        message.Body = mailBody.ToString();

        client.Send(message);
        return true;
    }
}
