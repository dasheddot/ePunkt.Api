using ePunkt.Api.Client;
using ePunkt.Portal.Models.ApplicantPersonalInformation;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantPersonalInformationController : ControllerBase
    {
        public ApplicantPersonalInformationController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }

        public async Task<ActionResult> Index()
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");
            return View(new IndexViewModel(await GetMandator(), applicant));
        }
    }
}
