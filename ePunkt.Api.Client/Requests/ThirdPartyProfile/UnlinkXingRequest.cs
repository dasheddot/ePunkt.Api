using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class UnlinkXingRequest : HttpRequestMessage<ApplicantResponse>
    {
        public UnlinkXingRequest(int id)
            : base(HttpMethod.Delete, "Applicant/Xing/" + id)
        {
        }
    }
}
