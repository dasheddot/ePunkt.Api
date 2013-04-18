using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class UnlinkXingRequest : HttpRequestMessage
    {
        public UnlinkXingRequest(int id)
            : base(HttpMethod.Delete, "Xing/" + id)
        {
        }
    }
}
