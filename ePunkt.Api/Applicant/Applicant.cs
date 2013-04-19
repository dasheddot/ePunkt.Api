using System.Collections.Generic;

namespace ePunkt.Api
{
    public class Applicant : ApplicantUpdateParameter
    {
        public int Id { get; set; }

        public IEnumerable<ApplicantThirdPartyProfile> ThirdPartyProfiles { get; set; }
        public IEnumerable<ApplicantDocument> Documents { get; set; }
        public IEnumerable<ApplicantCustomField> CustomFields { get; set; } 
    }

    public class ApplicantThirdPartyProfile
    {
        public string ThirdParty { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
    }

    public class ApplicantDocument
    {
        public string Name { get; set; }
        public string FileExtension { get; set; }
    }

    public class ApplicantCustomField : CustomField { }
}
