
namespace ePunkt.Portal.Models.Applicant
{
    public class IndexViewModel
    {
        public IndexViewModel(Api.Models.Applicant applicant)
        {
            Applicant = applicant;
            Completeness = new ApplicantCompleteness(applicant);
        }

        public Api.Models.Applicant Applicant { get; set; }
        public ApplicantCompleteness Completeness { get; set; }
    }
}