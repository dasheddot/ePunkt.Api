using ePunkt.Api.Responses;
using System.Collections.Generic;
using System.Linq;

namespace ePunkt.Portal.Models.Jobs
{
    public class IndexViewModel
    {

        public void BuildJobs(IEnumerable<JobResponse> jobs)
        {
            var result = jobs.Select(job => new Job
                {
                    Date = job.OnlineDateCorrected.ToShortDateString(),
                    Id = job.Id,
                    Location = job.Location,
                    SubTitle = job.SubTitle,
                    Title = job.Title
                }).ToList();
            Jobs = result;
        }

        public IEnumerable<RegionResponse> AvailableRegions { get; set; }
        public IEnumerable<string> AvailableJobProfiles { get; set; }
        public IEnumerable<string> FilteredRegions { get; set; }
        public IEnumerable<string> FilteredJobProfiles { get; set; }
        public string FilteredText { get; set; }

        public IEnumerable<Job> Jobs { get; set; }
        public int TotalJobsCount { get; set; }

        public bool DisplayJobProfileFilter { get; set; }
        public bool DisplayRegionFilter { get; set; }
        public bool DisplayFilter { get; set; }
        public bool MoveFilterToBottom { get; set; }
        public bool DisplayJobDate { get; set; }
        public bool DisplayJobLocation { get; set; }

        public class Job
        {
            public string Title { get; set; }
            public string SubTitle { get; set; }
            public string Location { get; set; }
            public string Date { get; set; }
            public int Id { get; set; }
        }
    }
}