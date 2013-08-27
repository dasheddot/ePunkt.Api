using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class SetPasswordRequest : PostJsonHttpRequestMessage<ApplicantSetPasswordResponse>
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
