
namespace ePunkt.Api.Parameters
{
    public class ApplicationCreateParameter
    {
        public ApplicationCreateParameter(int jobId, int applicantId)
        {
            JobId = jobId;
            ApplicantId = applicantId;
        }

        public int JobId { get; set; }
        public int ApplicantId { get; set; }
        public string Referrer { get; set; }
        public string ReferrerAdditionalText { get; set; }
        public bool RefreshApplicationWhenIfItAlreadyExists { get; set; }
    }
}
