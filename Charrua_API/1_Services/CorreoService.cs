using Charrua_API.DTOs.Correos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Charrua_API.Services
{
    public static class CorreoService
    {
        //private static string Host = "smtp.gmail.com";
        //public static int Puerto = 587;

        //private static string SenderName = "Codigo estudiante";
        //private static string Correo = "juan.cremona@gmail";
        //private static string Clave = "Esta tiene que ser generada al crear el correo";


        //public static bool Send(CorreoDTO correoDTO)
        //{
        //    try
        //    {
        //        var email = new MimeMessage();
        //        email.From.Add(new MailboxAddress(SenderName, Correo));
        //        email.To.Add(MailboxAddress.Parse(correoDTO.Para));
        //        email.Subject = correoDTO.Asunto;
        //        email.Body = new TextPart(TextFormat.Html)
        //        {
        //            Text = correoDTO.Contenido
        //        };
        //        var smtp = new SmtpClient();
        //        smtp.Connect(Host, Puerto, SecureSocketOptions.StartTls);
        //        smtp.Authenticate(Correo, Clave);
        //        smtp.Send(email);
        //        smtp.Disconnect(true);
        //        return true;

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}
