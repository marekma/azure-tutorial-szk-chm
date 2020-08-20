using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function
{
    public class BodyService : IBodyService
    {
        public async Task<string> ReadBodyAsync(HttpRequest req)
        {
            string result = null;
            using (StreamReader r = new StreamReader(req.Body))
            {
                result = await r.ReadToEndAsync();
            }
            return result;
        }
    }
}
