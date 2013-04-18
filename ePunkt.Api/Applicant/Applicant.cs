using System.Collections.Generic;

namespace ePunkt.Api
{
    public class Applicant : ApplicantUpdateParameter
    {
        public int Id { get; set; }

        public IEnumerable<ThirdPartyProfile> ThirdPartyProfiles { get; set; } 
    }

    public class ThirdPartyProfile
    {
        public string ThirdParty { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
    }
}
