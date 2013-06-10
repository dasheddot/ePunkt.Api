using ePunkt.Api.Models;

namespace ePunkt.Portal.Models.Application
{
    public class IndexViewModel
    {
        public IndexViewModel(Job job)
        {
            JobId = job.Id;
            Job = job;
        }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}