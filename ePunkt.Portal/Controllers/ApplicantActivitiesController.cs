using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.ApplicantActivities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantActivitiesController : ControllerBase
    {
        public ApplicantActivitiesController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }


        public async Task<ActionResult> Index()
        {
            var activities =  await new ApplicantActivitiesGetRequest(GetApplicantId()).LoadResult(ApiClient);
            var viewModel = new IndexViewModel().Fill(activities);
            return View(viewModel);
        }


        public ActionResult Create()
        {
            var viewModel = new EditViewModel().Prepare();
            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await new ApplicantActivityPutRequest(GetApplicantId(), new ApplicantActivityParameter
                {
                    Name = model.Name,
                    Start = new FlexDate(model.StartDay, model.StartMonth, model.StartYear),
                    End = new FlexDate(model.EndDay, model.EndMonth, model.EndYear)
                }).LoadResult(ApiClient);
                return RedirectToAction("Index");
            }
            model.Prepare();
            return View("Edit", model);
        }


        public async Task<ActionResult> Edit(int id)
        {
            var activity = await LoadActivity(id);
            if (activity == null)
                return RedirectToAction("Index");
            var viewModel = new EditViewModel().Prepare().Fill(activity);
            return View("Edit", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activity = await LoadActivity(id);
                activity.Name = model.Name;
                activity.Start = new FlexDate(model.StartDay, model.StartMonth, model.StartYear);
                activity.End = new FlexDate(model.EndDay, model.EndMonth, model.EndYear);
                await new ApplicantActivityPostRequest(id, activity).LoadResult(ApiClient);
                return RedirectToAction("Index");
            }
            model.Prepare();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var activity = await LoadActivity(id);
            if (activity != null)
                await new ApplicantActivityDeleteRequest(activity.Id).LoadResult(ApiClient);
            return RedirectToAction("Index");
        }

        private async Task<ApplicantActivityResponse> LoadActivity(int id)
        {
            var activities = await new ApplicantActivitiesGetRequest(GetApplicantId()).LoadResult(ApiClient);
            return activities.FirstOrDefault(x => x.Id == id);
        }
    }
}
