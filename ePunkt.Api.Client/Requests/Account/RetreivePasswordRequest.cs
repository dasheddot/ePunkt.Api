using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class RetreivePasswordRequest : HttpRequestMessage
    {
        public RetreivePasswordRequest(string email, Uri currentUri)
            : base(HttpMethod.Post, "RetreivePassword?email=" + email + "&url=" + currentUri)
        {
        }
    }
}
