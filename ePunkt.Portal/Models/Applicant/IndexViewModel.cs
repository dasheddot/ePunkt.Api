using ePunkt.Api.Responses;

namespace ePunkt.Portal.Models.Applicant
{
    public class IndexViewModel
    {
        public IndexViewModel(ApplicantResponse applicantResponse)
        {
            ApplicantResponse = applicantResponse;
            Completeness = new ApplicantCompleteness(applicantResponse);
        }

        public ApplicantResponse ApplicantResponse { get; set; }
        public ApplicantCompleteness Completeness { get; set; }
    }
}