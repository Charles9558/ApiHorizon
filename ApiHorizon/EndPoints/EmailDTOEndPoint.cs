using ApiHorizon.Models;
using ApiHorizon.Services;

namespace ApiHorizon.EndPoints
{
    public static class EmailDTOEndPoint
    {
        public static RouteGroupBuilder MapEmailDTOEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/email");

            //MapPost

            Group.MapPost("/newpass", async (Personnel p, IEmailService emailService) => {
                EmailDTO request=new EmailDTO();
                request.From = "charlesnana31127@gmail.com";
                request.To = p.Email;
                request.Subject = "Modification de mot de passe";
                request.Body = $"<p><b>{p.Pseudo}</b> votre nouveau mot de passe est <b>{p.Pass}</b>. Veillez le modifier immédiatement à votre prochaine connexion.</p>";
                await emailService.SendEmailAsync(request);
                return Results.Ok("Email envoyé !");
            });

            return Group;
        }
         
    }
}
