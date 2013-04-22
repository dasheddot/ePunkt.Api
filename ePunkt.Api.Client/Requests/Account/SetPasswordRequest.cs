using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class SetPasswordRequest : PostJsonHttpRequestMessage
    {
        public SetPasswordRequest(SetPasswordAfterRetreiveParameter param)
            : base("ChangePassword", param)
        {
        }
    }
}
