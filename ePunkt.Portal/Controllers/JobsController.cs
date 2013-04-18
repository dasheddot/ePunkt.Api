using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Portal.Models.Jobs;
using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ePunkt.Portal.Controllers
{
    public class JobsController : ControllerBase
    {
        private readonly LoadJobsService _jobsService;

        public JobsController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService jobsService)
            : base(apiClient, settings)
        {
            _jobsService = jobsService;
        }

        public async Task<ActionResult> Index(string filter, string jobProfiles, string regions)
        {
            var mandator = await GetMandator();
            var response = await _jobsService.LoadJobsForCurrentPortal(Request.Url, mandator);
            var jobs = response.Jobs.Filter(filter, jobProfiles, regions);

            var jobProfilesFilter = (jobProfiles ?? "").Split('|').Select(x => x.Trim()).Where(x => x.HasValue()).ToArray();
            var regionsFilter = (regions ?? "").Split('|').Select(x => x.Trim()).Where(x => x.HasValue()).ToArray();

            var model = new IndexViewModel
                {
                    AvailableJobProfiles = mandator.JobProfiles,
                    AvailableRegions = mandator.Regions,
                    TotalJobsCount = response.Jobs.Count(),
                    FilteredText = filter,
                    FilteredJobProfiles = jobProfilesFilter,
                    FilteredRegions = regionsFilter,
                    DisplayFilter = mandator.Settings.EnableFilterOnJobsList,
                    DisplayJobProfileFilter = mandator.Settings.EnableJobProfilesFilterOnJobsList,
                    DisplayRegionFilter = mandator.Settings.EnableRegionsFilterOnJobsList,
                    DisplayJobDate = mandator.Settings.DisplayDateInJobListing,
                    DisplayJobLocation = mandator.Settings.DisplayLocationInJobListing,
                    MoveFilterToBottom = mandator.Settings.MoveJobsFilterToBottomOnJobsList
                };
            model.BuildJobs(jobs.OrderByDescending(x => x.OnlineDateCorrected));

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string filter)
        {
            var mandator = await GetMandator();

            var selectedRegions = BuildSelectedRegionsString(mandator.Regions);
            selectedRegions = selectedRegions.Trim('|');

            var selectedJobProfiles = "";
            foreach (var jobProfile in mandator.JobProfiles)
                if (Request.Form[jobProfile].Is(true))
                    selectedJobProfiles += jobProfile + "|";
            selectedJobProfiles = selectedJobProfiles.Trim('|');

            return RedirectToAction("Index",
                                    new { filter, jobProfiles = selectedJobProfiles.HasValue() ? selectedJobProfiles : null, regions = selectedRegions.HasValue() ? selectedRegions : null });
        }

        private string BuildSelectedRegionsString(IEnumerable<Region> regions)
        {
            var selectedRegions = "";
            foreach (var region in regions)
            {
                if (Request.Form[region.Name].Is(true))
                    selectedRegions += region.Name + "|";
                selectedRegions += BuildSelectedRegionsString(region.Regions);
            }
            return selectedRegions;
        }

        public async Task<ActionResult> Job(int id)
        {
            var mandator = await GetMandator();
            var jobsResponse = await _jobsService.LoadJobsForCurrentPortal(Request.Url, mandator);

            var job = jobsResponse.Jobs.FirstOrDefault(x => x.Id == id);
            if (job == null)
                return RedirectToAction("Index");

            var fixUrlsService = new FixJobAdUrlsService(mandator);
            var model = new JobViewModel
                {
                    Html = fixUrlsService.ReplaceUrlsWithCurrent(job.Html, Request.Url),
                    DisplayBackLink = mandator.Settings.DisplayBackToOverviewLinkOnJobDetails
                };
            return View(model);
        }

        //cache it, but only for a very short duration, because otherwise the preview in eR would always load an old version
        [OutputCache(Duration = 30, VaryByParam = "id", Location = OutputCacheLocation.Server)]
        public async Task<ActionResult> SalaryImage(int id)
        {
            try
            {
                var response = await ApiClient.SendAndReadAsync<JobSalaryImageResponse>(new JobSalaryImageRequest(id));
                if (response != null && response.Image != null)
                    return new FileContentResult(Convert.FromBase64String(response.Image), MimeMapping.GetMimeMapping("JobSalaryImage" + id + ".jpg"));
            }
            catch
            {
                //do nothing here
            }
            throw new HttpException(404, "Job Salary Image not available.");
        }
    }
}
