using ePunkt.Api.Responses;

namespace ePunkt.Portal.Models.Application
{
    public class JobIsClosedViewModel
    {
        public JobIsClosedViewModel(JobResponse jobResponse)
        {
            JobId = jobResponse.Id;
            JobResponse = jobResponse;
        }

        public int JobId { get; set; }
        public JobResponse JobResponse { get; set; }
    }
}