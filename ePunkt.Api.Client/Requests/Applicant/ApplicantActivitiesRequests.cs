using System.Collections.Generic;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantActivitiesGetRequest : HttpRequestMessage<IEnumerable<ApplicantActivityResponse>>
    {
        public ApplicantActivitiesGetRequest(int applicantId)
            : base(HttpMethod.Get, "Applicant/Activities/" + applicantId)
        {
        }
    }
}
