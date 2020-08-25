using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Abstracts
{
    public interface IFileShareProvider
    {
        Task Upload(Stream content, string blobname);
        Task<string> Download(string blobname);
    }
}
