using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Abstracts
{
    public interface IBlobProvider
    {
        Task Upload(Stream content, string blobname);
        Task<string> Download(string blobname);
    }
}
