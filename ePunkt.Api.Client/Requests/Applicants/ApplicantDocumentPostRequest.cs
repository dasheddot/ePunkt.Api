using ePunkt.Api.Models;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentPostRequest : PostJsonHttpRequestMessage
    {
        public ApplicantDocumentPostRequest(int applicantId, DocumentContent file)
            : base("ApplicantDocument/" + applicantId, file)
        {
        }
    }
}
