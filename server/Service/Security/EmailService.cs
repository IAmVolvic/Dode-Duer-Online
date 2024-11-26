using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Service.Security;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _senderEmail = Environment.GetEnvironmentVariable("SMTPGMAIL_EMAIL")!;
    private readonly string _senderPassword = Environment.GetEnvironmentVariable("SMTPGMAIL_PASSWORD")!;

    public async Task SendTempPasswordToEmail(string recipientname, string recipientEmail, string recipientTempPassword)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Lotto Game", _senderEmail));
            message.To.Add(new MailboxAddress(recipientname, recipientEmail));
            message.Subject = "Welcome to Lotto Game!";
            
            string filePath = "../Service/Security/HTMLPresets/NewUser.html";  // Update the file path here
            string htmlTemplate = File.ReadAllText(filePath);
            string emailBody = htmlTemplate.Replace("{{variable_here}}", recipientTempPassword);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);
                await client.AuthenticateAsync(_senderEmail, _senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}