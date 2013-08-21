using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class SetPasswordRequest : PostJsonHttpRequestMessage
    {
        public SetPasswordRequest(SetPasswordAfterRequestParameter param)
            : base("Applicant/SetPassword", param)
        {
        }

        public SetPasswordRequest(int applicantId, SetPasswordParameter param)
            : base("Applicant/SetPassword/" + applicantId, param)
        {
        }
    }
}
