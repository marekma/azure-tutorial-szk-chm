using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Company.Function
{
    public interface IHeaderService
    {
        string GetCorrelationId(HttpRequest req);
        void SetCorellationId(HttpRequestHeaders headers, string value);
    }
}