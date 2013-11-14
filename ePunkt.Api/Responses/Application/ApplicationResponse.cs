using System.Collections.Generic;

namespace ePunkt.Api.Responses
{
    public class ApplicationResponse
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public int JobId { get; set; }
        public string Status { get; set; }
        public string StatusForApplicant { get; set; }
        public string StatusForContact { get; set; }
        public bool IsVisibleToApplicant { get; set; }
        public bool IsVisibleToContact { get; set; }
        public IEnumerable<ApplicationActivityResponse> AvailableActivities { get; set; }

        public class ApplicationActivityResponse
        {
            public string Name { get; set; }
            public string NameForApplicant { get; set; }
            public string NameForContact { get; set; }
            public bool IsVisibleToApplicant { get; set; }
            public ContactSharing ContactSharing { get; set; }
        }
    }

    public enum ContactSharing
    {
        None,
        Visible,
        VisibleAndExecuteWorkflow,
    }
}
