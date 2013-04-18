using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace ePunkt.Portal.Controllers
{
    public class AccountController : ControllerBase
    {
        public AccountController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(username, password));
            if (applicant != null)
            {
                FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);
                return RedirectToAction("Index", "Applicant");
            }

            ModelState.AddModelError("username", "Username or password is invalid");
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Jobs");
        }
    }
}
