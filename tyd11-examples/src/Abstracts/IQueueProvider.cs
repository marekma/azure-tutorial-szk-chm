using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Abstracts
{
    public interface IQueueProvider
    {
        Task SendMessage(Stream message);
        Task<string> ReceiveMessage();
    }
}
