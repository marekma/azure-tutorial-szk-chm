using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Company.Function
{
    public interface IBodyService
    {
        Task<string> ReadBodyAsync(HttpRequest req);
    }
}