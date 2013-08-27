using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Portal.Models.Applications;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly LoadJobsService _loadJobsService;

        public ApplicationsController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService loadJobsService)
            : base(apiClient, settings)
        {
            _loadJobsService = loadJobsService;
        }

        public async Task<ActionResult> Index()
        {
            var mandator = await GetMandator();
            var applications = (await new ApplicationsRequest(GetApplicantId()).LoadResult(ApiClient)).Where(x => x.IsVisibleToApplicant).ToList();
            var viewModel = await new IndexViewModel(applications).Build(mandator, _loadJobsService, Request.Url);
            return View(viewModel);
        }
    }
}
