using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class SetPasswordRequest : PostJsonHttpRequestMessage
    {
        public SetPasswordRequest(ApplicantSetPasswordAfterRequestParameter param)
            : base("Applicant/SetPassword", param)
        {
        }

        public SetPasswordRequest(int applicantId, ApplicantSetPasswordParameter param)
            : base("Applicant/SetPassword/" + applicantId, param)
        {
        }
    }
}
