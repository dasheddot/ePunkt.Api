using System;

namespace ePunkt.Api.Parameters
{
    public class SetPasswordAfterRequestParameter
    {
        public SetPasswordAfterRequestParameter(string email, string code, string newPassword, Uri url)
        {
            Email = email;
            Code = code;
            NewPassword = newPassword;
            Url = url;
        }

        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public Uri Url { get; set; }
    }
}
