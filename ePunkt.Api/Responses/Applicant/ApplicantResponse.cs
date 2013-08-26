using System;
using System.Collections.Generic;
using ePunkt.Api.Parameters;

namespace ePunkt.Api.Responses
{
    public class ApplicantResponse : ApplicantUpdateParameter
    {
        public int Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfUpdate { get; set; }

        public IEnumerable<ApplicantThirdPartyProfileResponse> ThirdPartyProfiles { get; set; }
        public IEnumerable<ApplicantDocumentOverviewResponse> Documents { get; set; }
        public IEnumerable<ApplicantCustomFieldResponse> CustomFields { get; set; } 
    }

    public class ApplicantThirdPartyProfileResponse
    {
        public string ThirdParty { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
    }

    public class ApplicantDocumentOverviewResponse
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string FileExtension { get; set; }
    }

    public class ApplicantCustomFieldResponse : CustomFieldResponse { }
}
