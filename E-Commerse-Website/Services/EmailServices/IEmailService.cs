using E_Commerse_Website.Services.DTO;

namespace E_Commerse_Website.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO request);
        void SendEmailAsyncFireAndForget(EmailDTO request);
    }
}
