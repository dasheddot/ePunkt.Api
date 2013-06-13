using ePunkt.Api.Client;
using ePunkt.Portal.Models.ApplicantPersonalInformation;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantPersonalInformationController : ControllerBase
    {
        private readonly UpdateApplicantService _updateApplicantService;

        public ApplicantPersonalInformationController(ApiHttpClient apiClient, CustomSettings settings, UpdateApplicantService updateApplicantService)
            : base(apiClient, settings)
        {
            _updateApplicantService = updateApplicantService;
        }

        public async Task<ActionResult> Index()
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");

            var model = new IndexViewModel();
            model.Prepare(await GetMandator(), applicant);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");

            if (ModelState.IsValid)
                applicant = await _updateApplicantService.UpdatePersonalInformation(ApiClient, applicant, model);

            model.Prepare(await GetMandator(), applicant);
            return View(model);
        }

    }
}
