using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ConfirmRequestPasswordRequest : HttpRequestMessage
    {
        public ConfirmRequestPasswordRequest(string email, string code)
            : base(HttpMethod.Get, "Applicant?email=" + email + "&code=" + code)
        {
        }
    }
}
