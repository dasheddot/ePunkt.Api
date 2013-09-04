using System.Collections.Generic;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantJobProfilePutRequest : PutJsonHttpRequestMessage<IEnumerable<string>>
    {
        public ApplicantJobProfilePutRequest(int applicantId, string jobProfileName)
            : base("Applicant/JobProfile/" + applicantId, jobProfileName)
        {
        }
    }

    public class ApplicantJobProfileDeleteRequest : DeleteJsonHttpRequestMessage<IEnumerable<string>>
    {
        public ApplicantJobProfileDeleteRequest(int applicantId, string jobProfileName)
            : base("Applicant/JobProfile/" + applicantId, jobProfileName)
        {
        }
    }
}
