using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantsRequest : HttpRequestMessage
    {
        public ApplicantsRequest(string email)
            : base(HttpMethod.Get, "Applicants?email=" + email)
        {
        }
    }
}
