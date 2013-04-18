using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class UnlinkLinkedInRequest : HttpRequestMessage
    {
        public UnlinkLinkedInRequest(int id)
            : base(HttpMethod.Delete, "LinkedIn/" + id)
        {
        }
    }
}
