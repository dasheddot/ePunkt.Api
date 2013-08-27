using System;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class RequestPasswordRequest : HttpRequestMessage<ApplicantSetPasswordResponse>
    {
        public RequestPasswordRequest(string email, Uri currentUri)
            : base(HttpMethod.Post, "Applicant/RequestPassword?email=" + email + "&url=" + currentUri)
        {
        }
    }
}
