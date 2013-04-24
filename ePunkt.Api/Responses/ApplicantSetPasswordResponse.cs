using System.Collections.Generic;

namespace ePunkt.Api.Responses
{
    public class ApplicantSetPasswordResponse
    {
        public IEnumerable<Error> Errors { get; set; }

        public enum Error
        {
            PasswordPolicySymbols,
            PasswordPolicyNumeric,
            PasswordPolicyUppercase,
            PasswordPolicyLength,
            InvalidCode,
            InvalidOldPassword
        }
    }
}
