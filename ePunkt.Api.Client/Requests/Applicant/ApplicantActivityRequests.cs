using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantActivityGetRequest : HttpRequestMessage<ApplicantActivityResponse>
    {
        public ApplicantActivityGetRequest(int activityId)
            : base(HttpMethod.Get, "Applicant/Activity/" + activityId)
        {
        }
    }

    public class ApplicantActivityPutRequest : PutJsonHttpRequestMessage<ApplicantActivityResponse>
    {
        public ApplicantActivityPutRequest(int applicantId, ApplicantActivityParameter activity)
            : base("Applicant/Activity/" + applicantId, activity)
        {
        }
    }

    public class ApplicantActivityPostRequest : PostJsonHttpRequestMessage<ApplicantActivityResponse>
    {
        public ApplicantActivityPostRequest(int activityId, ApplicantActivityParameter activity)
            : base("Applicant/Activity/" + activityId, activity)
        {
        }
    }

    public class ApplicantActivityDeleteRequest : HttpRequestMessage<ApplicantActivityResponse>
    {
        public ApplicantActivityDeleteRequest(int activityId)
            : base(HttpMethod.Delete, "applicant/activity/" + activityId)
        {
        }
    }
}
