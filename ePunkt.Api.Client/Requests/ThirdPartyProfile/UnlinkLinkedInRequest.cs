using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class UnlinkLinkedInRequest : HttpRequestMessage<ApplicantResponse>
    {
        public UnlinkLinkedInRequest(int id)
            : base(HttpMethod.Delete, "Applicant/LinkedIn/" + id)
        {
        }
    }
}
