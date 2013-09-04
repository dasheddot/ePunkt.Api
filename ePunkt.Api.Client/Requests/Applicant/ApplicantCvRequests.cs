using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantCvGetRequest : HttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantCvGetRequest(int applicantId, bool includeContent = true)
            : base(HttpMethod.Get, "Applicant/Cv/" + applicantId + "?includeContent=" + includeContent)
        {
        }
    }

    public class ApplicantCvPutRequest : PutJsonHttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantCvPutRequest(int applicantId, ApplicantDocumentParameter document)
            : base("Applicant/Cv/" + applicantId, document)
        {
        }
    }

    public class ApplicantCvPostRequest : PostJsonHttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantCvPostRequest(int applicantId, ApplicantDocumentParameter document)
            : base("Applicant/Cv/" + applicantId, document)
        {
        }
    }

    public class ApplicantCvDeleteRequest : HttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantCvDeleteRequest(int applicantId)
            : base(HttpMethod.Delete, "Applicant/Cv/" + applicantId)
        {
        }
    }
}
