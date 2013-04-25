using ePunkt.Api.Client;
using System.Threading.Tasks;
using System.Web.Mvc;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Portal.Models.Applicant;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantController : ControllerBase
    {
        public ApplicantController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }

        public async Task<ActionResult> Index()
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");
            return View(new IndexViewModel(applicant));
        }

        [HttpPost]
        public async Task<ActionResult> Index(bool? active, bool? newsletter, bool? matchingJobs)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
            {
                if (active.HasValue)
                    applicant.IsActive = active.Value;
                if (newsletter.HasValue)
                    applicant.EnableNewsletter = newsletter.Value;
                if (matchingJobs.HasValue)
                    applicant.EnableMatchingJobsAutoMail = matchingJobs.Value;
                await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(applicant.Id, applicant));
            }

            return RedirectToAction("Index");
        }
    }
}
