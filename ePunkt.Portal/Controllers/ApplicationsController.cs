using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Applications;
using System.Collections.Generic;
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
            var applications = (await ApiClient.SendAndReadAsync<IEnumerable<ApplicationResponse>>(new ApplicationsRequest(GetApplicantId()))).Where(x => x.IsVisibleToApplicant).ToList();
            var viewModel = await new IndexViewModel(applications).Build(mandator, _loadJobsService, Request.Url);
            return View(viewModel);
        }
    }
}
