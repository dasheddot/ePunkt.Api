using ePunkt.Api.Responses;
using System.Collections.Generic;

namespace ePunkt.Portal.Models.Applicant
{
    public class IndexViewModel
    {
        public IndexViewModel(ApplicantResponse applicantResponse, IEnumerable<ApplicantDocumentResponse> documents)
        {
            ApplicantResponse = applicantResponse;
            Completeness = new ApplicantCompleteness(applicantResponse, documents);
        }

        public ApplicantResponse ApplicantResponse { get; set; }
        public ApplicantCompleteness Completeness { get; set; }
    }
}