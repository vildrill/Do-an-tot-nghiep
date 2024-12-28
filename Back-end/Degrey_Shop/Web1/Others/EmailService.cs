using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string toAddress, string subject, string body)
    {
        var fromAddress = new MailAddress(_configuration["EmailSettings:FromEmailAddress"]);
        var toAddr = new MailAddress(toAddress);
        string smtpServer = _configuration["EmailSettings:SMTPServer"];
        int smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
        string username = _configuration["EmailSettings:SMTPUsername"];
        string password = _configuration["EmailSettings:SMTPPassword"];

        var smtp = new SmtpClient
        {
            Host = smtpServer,
            Port = smtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(username, password)
        };

        using (var message = new MailMessage(fromAddress, toAddr)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true // Nếu nội dung có HTML
        })
        {
            smtp.Send(message);
        }
    }
}
