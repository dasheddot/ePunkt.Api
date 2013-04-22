using System;

namespace ePunkt.Api.Parameters
{
    public class SetPasswordAfterRetreiveParameter
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public Uri Url { get; set; }
    }
}
