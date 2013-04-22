using System.Collections.Generic;
using ePunkt.Api.Models;

namespace ePunkt.Api.Responses
{
    public class ApplicantSetPasswordResponse
    {
        public Applicant Applicant { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public enum Error
        {
            PasswordPolicySymbols,
            PasswordPolicyNumeric,
            PasswordPolicyUppercase,
            PasswordPolicyLength
        }
    }
}
