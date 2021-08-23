using System.Threading.Tasks;

namespace CardFile.WebAPI.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}