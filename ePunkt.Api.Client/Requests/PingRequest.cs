using System.Net.Http;
using System.Net.Http.Formatting;

namespace ePunkt.Api.Client.Requests
{
    public class PingRequest : HttpRequestMessage
    {
        public PingRequest()
            : base(HttpMethod.Get, "Ping")
        {
        }

        public PingRequest(string message)
            : base(HttpMethod.Post, "Ping")
        {
            Content = new ObjectContent(message.GetType(), message, new JsonMediaTypeFormatter());
        }
    }
}
