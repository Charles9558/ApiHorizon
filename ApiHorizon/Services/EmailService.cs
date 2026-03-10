using ApiHorizon.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace ApiHorizon.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;	
	    public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(request.From));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            try
            {
                using var smtp = new SmtpClient();

                // Connexion sécurisée
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Authentification (mot de passe stocké de façon sécurisée idéalement)
                var smtpUser = "charlesnana31127@gmail.com";
                var smtpPass = "ds4p fezb gfzu cs37 jupf dzf3 vskw m73i";// à configurer sur votre machine

                await smtp.AuthenticateAsync(smtpUser, smtpPass);

                // Envoi du message
                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log ou afficher l'erreur en détail
                Console.WriteLine($"Erreur d'envoi email : {ex.Message}");
                throw; // rethrow si besoin
            }
        }

    }
    
    }
