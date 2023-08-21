using System.Net.Mail;
using System.Net;


namespace Charrua_API._1_Services
{
    public class EmailServiceImpl : Charrua_API._1_Services.EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailServiceImpl(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task<bool> SendConfirmationEmail(string email, string confirmationCode)
        {
            string confirmationLink = $"https://localhost:7076/confirm?code={confirmationCode}";//aca va endpoint de recepcion

            string body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Confirmación de Registro</title>
            </head>
            <body>
                <h1>¡Bienvenido a nuestra Casa de Campo!</h1>
                <p>Por favor, haga click en el siguiente enlace para confirmar su registro:</p>
                <a href='{confirmationLink}'>Confirmar Registro</a>
            </body>
            </html>
            ";

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("juan.cremona@gmail.com"); //aca va el email
            mail.To.Add(email);
            mail.Subject = "Confirmación de Registro de tu cuenta en Charrua";
            mail.Body = body;
            mail.IsBodyHtml = true;



            var smtp = new SmtpClient();
            smtp.Port = _smtpPort;
            smtp.Host = _smtpServer;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            smtp.EnableSsl = true;
            
            await smtp.SendMailAsync(mail);

            return true;

        }

    }
}


