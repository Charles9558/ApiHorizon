using ApiHorizon.Models;

namespace ApiHorizon.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO request);
    }
}
