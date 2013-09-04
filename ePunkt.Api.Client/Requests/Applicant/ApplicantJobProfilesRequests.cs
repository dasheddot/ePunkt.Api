using System.Collections.Generic;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantJobProfilesGetRequest : HttpRequestMessage<IEnumerable<string>>
    {
        public ApplicantJobProfilesGetRequest(int applicantId)
            : base(HttpMethod.Get, "Applicant/JobProfiles/" + applicantId)
        {
        }
    }
}
