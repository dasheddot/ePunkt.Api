using ePunkt.Api.Models;

namespace ePunkt.Portal.Models.Application
{
    public class RefreshViewModel
    {
        public RefreshViewModel(Job job)
        {
            JobId = job.Id;
            Job = job;
        }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}