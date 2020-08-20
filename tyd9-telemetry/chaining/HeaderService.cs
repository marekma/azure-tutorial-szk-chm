using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Company.Function
{
    public class HeaderService : IHeaderService
    {
        private readonly string CORELLATION_ID = "CorrellationId";

        public string GetCorrelationId(HttpRequest req)
        {
            return req.Headers[CORELLATION_ID];
        }

        public void SetCorellationId(HttpRequestHeaders headers, string value)
        {
            headers.Add(CORELLATION_ID, value);
        }
    }
}
