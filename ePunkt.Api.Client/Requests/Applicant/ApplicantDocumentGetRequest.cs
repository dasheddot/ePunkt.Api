using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentGetRequest : HttpRequestMessage
    {
        public ApplicantDocumentGetRequest(int applicantId, string name, string type)
            : base(HttpMethod.Get, "Applicant/Document/" + applicantId + "?name=" + name + "&type=" + type)
        {
        }
    }
}
