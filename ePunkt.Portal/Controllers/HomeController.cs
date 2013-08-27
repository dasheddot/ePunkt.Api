using ePunkt.Api.Client;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    public class HomeController : ControllerBase
    {

        public HomeController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }

        public ActionResult Index(int? job)
        {
            if (job.HasValue)
            {
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Application", new {job});
                return RedirectToAction("Login", "Account", new {job});
            }

            return RedirectToAction("Index", "Jobs");
        }
    }
}
