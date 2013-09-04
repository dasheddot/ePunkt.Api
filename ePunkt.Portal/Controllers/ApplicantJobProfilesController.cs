using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Portal.Models.ApplicantJobProfiles;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantJobProfilesController : ControllerBase
    {

        public ApplicantJobProfilesController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }


        public async Task<ActionResult> Index()
        {
            var jobProfiles = await new ApplicantJobProfilesGetRequest(GetApplicantId()).LoadResult(ApiClient);
            var viewModel = new IndexViewModel().Fill(await GetMandator(), jobProfiles);
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
                await new ApplicantJobProfilePutRequest(GetApplicantId(), model.SelectedJobProfile).LoadResult(ApiClient);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string name)
        {
            if (ModelState.IsValid)
                await new ApplicantJobProfileDeleteRequest(GetApplicantId(), name).LoadResult(ApiClient);
            return RedirectToAction("Index");
        }
    }
}
