using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ConfirmRequestPasswordRequest : HttpRequestMessage
    {
        public ConfirmRequestPasswordRequest(string email, string code)
            : base(HttpMethod.Get, "RequestPassword?email=" + email + "&code=" + code)
        {
        }
    }
}
