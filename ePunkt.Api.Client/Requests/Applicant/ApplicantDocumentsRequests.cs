using ePunkt.Api.Responses;
using System.Collections.Generic;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentsGetRequest : HttpRequestMessage<IEnumerable<ApplicantDocumentResponse>>
    {
        public ApplicantDocumentsGetRequest(int applicantId)
            : base(HttpMethod.Get, "Applicant/Documents/" + applicantId + "?includeContent=false")
        {
        }
    }
}
