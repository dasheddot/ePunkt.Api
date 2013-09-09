using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class RequestPasswordRequest : HttpRequestMessage<string>
    {
        public RequestPasswordRequest(string email, Uri currentUri)
            : base(HttpMethod.Post, "Applicant/RequestPassword?email=" + email + "&url=" + currentUri)
        {
        }
    }
}
