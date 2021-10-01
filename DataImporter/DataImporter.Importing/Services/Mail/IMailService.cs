using System.Threading.Tasks;

namespace DataImporter.Importing.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
