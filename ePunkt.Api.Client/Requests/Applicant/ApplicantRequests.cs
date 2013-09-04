using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantGetRequest : HttpRequestMessage<ApplicantResponse>
    {

        public ApplicantGetRequest(string usernameOrEmail, string password)
            : base(HttpMethod.Get, "Applicant?usernameOrEmail=" + usernameOrEmail + "&password=" + password)
        {
        }

        public ApplicantGetRequest(string thirdPartyIdentifier, ThirdParty thirdParty)
            : base(HttpMethod.Get, "Applicant?identifier=" + thirdPartyIdentifier + "&thirdParty=" + thirdParty)
        {
        }

        public ApplicantGetRequest(int id)
            : base(HttpMethod.Get, "Applicant/" + id)
        {
        }
    }


    public class ApplicantPutRequest : PutJsonHttpRequestMessage<ApplicantResponse>
    {
        public ApplicantPutRequest(ApplicantParameter applicant)
            : base("Applicant/", applicant)
        {
        }
    }


    public class ApplicantPostRequest : PostJsonHttpRequestMessage<ApplicantResponse>
    {
        public ApplicantPostRequest(int applicantId, ApplicantParameter applicant)
            : base("Applicant/" + applicantId, applicant)
        {
        }
    }
}
