using ePunkt.Api.Responses;

namespace ePunkt.Portal.Models.Application
{
    public class RefreshViewModel
    {
        public RefreshViewModel(JobResponse jobResponse)
        {
            JobId = jobResponse.Id;
            JobResponse = jobResponse;
        }

        public int JobId { get; set; }
        public JobResponse JobResponse { get; set; }
    }
}