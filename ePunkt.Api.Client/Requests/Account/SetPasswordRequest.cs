using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class SetPasswordRequest : PostJsonHttpRequestMessage
    {
        public SetPasswordRequest(SetPasswordAfterRequestParameter param)
            : base("SetPassword", param)
        {
        }

        public SetPasswordRequest(int applicantId, SetPasswordParameter param)
            : base("SetPassword/" + applicantId, param)
        {
        }
    }
}
