using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentGetRequest : HttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantDocumentGetRequest(int documentId, bool includeContent = true)
            : base(HttpMethod.Get, "Applicant/Document/" + documentId + "?includeContent=" + includeContent)
        {
        }
    }

    public class ApplicantDocumentPutRequest : PutJsonHttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantDocumentPutRequest(int applicantId, ApplicantDocumentParameter document)
            : base("Applicant/Document/" + applicantId, document)
        {
        }
    }

    public class ApplicantDocumentPostRequest : PostJsonHttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantDocumentPostRequest(int documentId, ApplicantDocumentParameter document)
            : base("Applicant/Document/" + documentId, document)
        {
        }
    }

    public class ApplicantDocumentDeleteRequest : HttpRequestMessage<ApplicantDocumentResponse>
    {
        public ApplicantDocumentDeleteRequest(int documentId)
            : base(HttpMethod.Delete, "Applicant/Document/" + documentId)
        {
        }
    }
}
