using E_Commerse_Website.Services.DTO;
using E_Commerse_Website.Services.EmailServices;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailDTO request)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailServices").GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("EmailServices").GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("EmailServices").GetSection("EmailUsername").Value, _config.GetSection("EmailServices").GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent to: {email}", request.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email sending failed for: {email}", request.To);
        }
    }

    public void SendEmailAsyncFireAndForget(EmailDTO request)
    {
        _logger.LogInformation("SendEmailAsyncFireAndForget called for: {email}", request.To);
        Task.Run(() => SendEmailAsync(request));
    }
}
