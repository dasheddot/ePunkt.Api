using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class ConfirmRequestPasswordRequest : HttpRequestMessage<ApplicantResponse>
    {
        public ConfirmRequestPasswordRequest(string email, string code)
            : base(HttpMethod.Get, "Applicant?email=" + email + "&code=" + code)
        {
        }
    }
}
