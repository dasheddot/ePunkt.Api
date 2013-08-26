using System.Threading.Tasks;
using ePunkt.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ePunkt.Portal.Models.Applications
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<ApplicationResponse> applicationResponses)
        {
            Applications = (from x in applicationResponses
                            orderby x.StatusForApplicant
                            select new Application
                            {
                                JobId = x.JobId,
                                Status = x.StatusForApplicant
                            }).ToList();
        }

        public async Task<IndexViewModel> Build(MandatorResponse mandator, LoadJobsService loadJobsService, Uri requestUrl)
        {
            var publishedJobs = await loadJobsService.LoadJobsForCurrentPortal(requestUrl, mandator);

            foreach (var application in Applications)
            {
                if (publishedJobs.Jobs.Any(x => x.Id == application.JobId))
                {
                    application.Job = publishedJobs.Jobs.Single(x => x.Id == application.JobId).Title;
                    application.JobIsPublished = true;
                }
                else
                {
                    application.Job = (await loadJobsService.LoadSingleJob(application.JobId)).Title;
                    application.JobIsPublished = false;
                }
            }

            return this;
        }

        public IList<Application> Applications { get; set; }

        public class Application
        {
            public string Job { get; set; }
            public int JobId { get; set; }
            public bool JobIsPublished { get; set; }
            public string Status { get; set; }
        }
    }
}