using System.Net.Http;
using System.Net.Http.Formatting;
using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantRequest : HttpRequestMessage
    {

        public ApplicantRequest(string usernameOrEmail, string password)
            : base(HttpMethod.Get, "Applicant?usernameOrEmail=" + usernameOrEmail + "&password=" + password)
        {
        }

        public ApplicantRequest(string thirdPartyIdentifier, ThirdParty thirdParty)
            : base(HttpMethod.Get, "Applicant?identifier=" + thirdPartyIdentifier + "&thirdParty=" + thirdParty)
        {
        }

        public ApplicantRequest(int id)
            : base(HttpMethod.Get, "Applicant/" + id)
        {
        }

        public ApplicantRequest(int id, ApplicantUpdateParameter applicant)
            : base(HttpMethod.Post, "Applicant/" + id)
        {
            Content = new ObjectContent(typeof (ApplicantUpdateParameter), applicant, new JsonMediaTypeFormatter());
        }

        public ApplicantRequest(ApplicantCreateParameter applicant)
            : base(HttpMethod.Put, "Applicant")
        {
            Content = new ObjectContent(typeof(ApplicantCreateParameter), applicant, new JsonMediaTypeFormatter());
        }
    }
}
