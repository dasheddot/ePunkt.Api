using System.Collections.Generic;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantsRequest : HttpRequestMessage<IEnumerable<ApplicantResponse>>
    {
        public ApplicantsRequest(string email)
            : base(HttpMethod.Get, "Applicants?email=" + email)
        {
        }
    }
}
