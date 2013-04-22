using System.Net.Http;
using System.Net.Http.Formatting;
using ePunkt.Api.Models;

namespace ePunkt.Api.Client.Requests
{
    public class ApiKeyRequest : HttpRequestMessage
    {
        public ApiKeyRequest(ApiKey apiKey)
            : base(HttpMethod.Post, "Auth")
        {
            Content = new ObjectContent(typeof (ApiKey), apiKey, new JsonMediaTypeFormatter());
        }
    }
}
