namespace Charrua_API._1_Services
{
    public interface EmailService
    {

        Task<bool> SendConfirmationEmail(string email, string confirmationCode);
    }
}
