using System.Threading.Tasks;

namespace PulseLogicHMR.Core.Utilities.Messaging
{
    public interface IMessagingService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
        Task<bool> SendWhatsAppAsync(string phoneNumber, string message);
    }
}
