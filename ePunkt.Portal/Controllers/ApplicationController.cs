using System.Linq;
using ePunkt.Api.Client;
using System.Threading.Tasks;
using System.Web.Mvc;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Application;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly LoadJobsService _jobsService;

        public ApplicationController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService jobsService)
            : base(apiClient, settings)
        {
            _jobsService = jobsService;
        }

        public async Task<ActionResult> Index(int job, bool? ignoreDuplicate)
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");

            var jobObject = await GetJob(_jobsService, job);
            if (jobObject == null)
                return RedirectToAction("Index", "Jobs");

            var parameter = new ApplicationCreateParameter(job, applicant.Id)
                {
                    RefreshApplicationWhenIfItAlreadyExists = ignoreDuplicate.HasValue && ignoreDuplicate.Value
                };
            var result = await ApiClient.SendAndReadAsync<ApplicationCreateResponse>(new CreateApplicationRequest(parameter));

            if (result.Errors.Any(x => x == ApplicationCreateResponse.Error.JobDoesNotExist || x == ApplicationCreateResponse.Error.ApplicantDoesNotExist))
                return RedirectToAction("Index", "Jobs");

            if (result.Errors.Any(x => x == ApplicationCreateResponse.Error.ApplicationAlreadyExists))
                return RedirectToAction("Refresh", "Application", new { job });

            if (result.Errors.Any(x => x == ApplicationCreateResponse.Error.ApplicationAlreadyExists))
                return RedirectToAction("JobIsClosed", "Application", new { job });

            return View(new IndexViewModel(jobObject));
        }

        [HttpPost]
        public ActionResult Index(int job)
        {
            return RedirectToAction("Index", "Applicant");
        }

        public async Task<ActionResult> Refresh(int job)
        {
            var jobObject = await GetJob(_jobsService, job);
            if (jobObject == null)
                return RedirectToAction("Index", "Jobs");

            return View(new RefreshViewModel(jobObject));
        }

        public async Task<ActionResult> JobIsClosed(int job)
        {
            var jobObject = await GetJob(_jobsService, job);
            if (jobObject == null)
                return RedirectToAction("Index", "Jobs");

            return View(new JobIsClosedViewModel(jobObject));
        }
    }
}
