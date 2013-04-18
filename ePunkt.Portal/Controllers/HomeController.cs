using System.Text;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using System;
using System.Diagnostics;
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

        public async Task<ActionResult> Index()
        {
            try
            {
                var pingResponse = await ApiClient.SendAndReadAsync<string>(new PingRequest());
                Debug.WriteLine(pingResponse + " (" + ApiClient.ElapsedMillisecondsInLastCall + "ms)");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to ping API endpoint", ex);
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
