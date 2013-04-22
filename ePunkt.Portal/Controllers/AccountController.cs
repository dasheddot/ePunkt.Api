using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Utilities;
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

            ModelState.AddModelError("username", "Username or password is invalid.");
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Jobs");
        }

        [HttpGet]
        public ActionResult RetreivePassword(string email, string code)
        {
            if (code.IsNoE())
                return View("RetreivePasswordStep1");

            return View("RetreivePasswordStep2");
        }

        [HttpPost]
        public async Task<ActionResult> RetreivePassword(string email, string code, string password1, string password2)
        {
            if (code.IsNoE())
            {
                await ApiClient.SendAndReadAsync<Applicant>(new RetreivePasswordRequest(email, Request.Url));
                return View("RetreivePasswordStep1Success");
            }

            if (password1.IsNoE())
                ModelState.AddModelError("password1", "Error-PasswordIsEmpty");
            if (password1 != password2)
                ModelState.AddModelError("password2", "Error-PasswordsDontMatch");

            if (ModelState.IsValid)
            {
                var requestParam = new SetPasswordAfterRetreiveParameter
                    {
                        Code = code,
                        Email = email,
                        NewPassword = password1,
                        Url = Request.Url
                    };
                var result = await ApiClient.SendAndReadAsync<ApplicantSetPasswordResponse>(new SetPasswordRequest(requestParam));
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("password1", "Error-" + error);

                if (result.Applicant == null)
                    ModelState.AddModelError("password1", "Error-InvalidCode");

                if (ModelState.IsValid)
                    return View("RetreivePasswordStep2Success");

            }

            return View("RetreivePasswordStep2");
        }
    }
}
