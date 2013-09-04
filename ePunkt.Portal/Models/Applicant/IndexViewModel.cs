using ePunkt.Api.Responses;
using System.Collections.Generic;

namespace ePunkt.Portal.Models.Applicant
{
    public class IndexViewModel
    {
        public IndexViewModel(ApplicantResponse applicantResponse, IEnumerable<ApplicantDocumentResponse> documents, IEnumerable<string> jobProfiles)
        {
            ApplicantResponse = applicantResponse;
            Completeness = new ApplicantCompleteness(applicantResponse, documents, jobProfiles);
        }

        public ApplicantResponse ApplicantResponse { get; set; }
        public ApplicantCompleteness Completeness { get; set; }
    }
}