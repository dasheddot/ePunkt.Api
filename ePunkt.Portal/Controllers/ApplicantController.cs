using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Portal.Models.Applicant;
using System.Threading.Tasks;
using System.Web.Mvc;

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

            var documents = await new ApplicantDocumentsGetRequest(applicant.Id).LoadResult(ApiClient);
            return View(new IndexViewModel(applicant, documents));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                await new ApplicantPostRequest(applicant.Id, applicant).LoadResult(ApiClient);
            }

            return RedirectToAction("Index");
        }
    }
}
