using ePunkt.Api.Client;
using System;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<ActionResult> AreYouThere()
        {
            try
            {
                var areYouThere = await ApiClient.GetAsync("Home/AreYouThere");
                var areYouThereResponse = await areYouThere.Content.ReadAsStringAsync();

                return new ContentResult
                    {
                        Content = areYouThereResponse,
                        ContentEncoding = Encoding.Unicode,
                        ContentType = "text/plain"
                    };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to ping API endpoint", ex);
            }
        }
    }
}
