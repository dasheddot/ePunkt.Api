using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentDeleteRequest : HttpRequestMessage
    {
        public ApplicantDocumentDeleteRequest(int applicantId, string name, string type)
            : base(HttpMethod.Delete, "Applicant/Document/" + applicantId + "?name=" + name + "&type=" + type)
        {
        }
    }
}
