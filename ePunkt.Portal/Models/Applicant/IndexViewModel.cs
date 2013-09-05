using ePunkt.Api.Responses;
using System.Collections.Generic;

namespace ePunkt.Portal.Models.Applicant
{
    public class IndexViewModel
    {
        public IndexViewModel(ApplicantResponse applicantResponse, IEnumerable<ApplicantDocumentResponse> documents, IEnumerable<string> jobProfiles, IEnumerable<ApplicantActivityResponse> activities)
        {
            ApplicantResponse = applicantResponse;
            Completeness = new ApplicantCompleteness(applicantResponse, documents, jobProfiles, activities);
        }

        public ApplicantResponse ApplicantResponse { get; set; }
        public ApplicantCompleteness Completeness { get; set; }
    }
}