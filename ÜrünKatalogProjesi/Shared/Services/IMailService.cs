using MezuniyetProjesi.Model;
using System.Threading.Tasks;
using static MezuniyetProjesi.Jobs.SendEmailJob;

namespace MezuniyetProjesi.Shared.Services
{
    public interface IMailService
    {
        Task<string> SendLogInEmailAsync(string recipientEmail, string recipientFirstName);
        Task<string> SendRegisterEmailAsync(string recipientEmail, string recipientFirstName);
    }
}
