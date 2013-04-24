using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class RequestPasswordRequest : HttpRequestMessage
    {
        public RequestPasswordRequest(string email, Uri currentUri)
            : base(HttpMethod.Post, "RequestPassword?email=" + email + "&url=" + currentUri)
        {
        }
    }
}
