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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password)
        {
            var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(username, password));
            if (applicant != null)
            {
                FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);
                return RedirectToAction("Index", "Applicant");
            }

            ModelState.AddModelError("username", @"Error-InvalidUsernameOrPassword");
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Jobs");
        }

        #region ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string oldPassword, string password1, string password2)
        {
            if (password1.IsNoE())
                ModelState.AddModelError("password1", @"Error-PasswordIsEmpty");
            if (password1 != password2)
                ModelState.AddModelError("password2", @"Error-PasswordsDontMatch");

            if (ModelState.IsValid)
            {
                var requestParam = new SetPasswordParameter(oldPassword, password1, Request.Url);
                var result = await ApiClient.SendAndReadAsync<ApplicantSetPasswordResponse>(new SetPasswordRequest(GetApplicantId(), requestParam));
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("password1", @"Error-" + error);

                if (ModelState.IsValid)
                    return View("ChangePasswordSuccess");
            }

            return View();
        }
        #endregion

        #region RequestPassword

        [HttpGet]
        public async Task<ActionResult> RequestPassword(string email, string code)
        {
            if (code.IsNoE())
                return View("RequestPasswordStep1");

            //check if the code really works, to display an early error message if not
            var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ConfirmRequestPasswordRequest(email, code));
            if (applicant == null || applicant.Id <= 0)
                ModelState.AddModelError("password1", @"Error-InvalidCode");

            return View("RequestPasswordStep2");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestPassword(string email, string code, string password1, string password2)
        {
            if (code.IsNoE())
            {
                await ApiClient.SendAndReadAsync<string>(new RequestPasswordRequest(email, Request.Url));
                return View("RequestPasswordStep1Success");
            }

            if (password1.IsNoE())
                ModelState.AddModelError("password1", @"Error-PasswordIsEmpty");
            if (password1 != password2)
                ModelState.AddModelError("password2", @"Error-PasswordsDontMatch");

            if (ModelState.IsValid)
            {
                var requestParam = new SetPasswordAfterRequestParameter(email, code, password1, Request.Url);
                var result = await ApiClient.SendAndReadAsync<ApplicantSetPasswordResponse>(new SetPasswordRequest(requestParam));
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("password1", @"Error-" + error);

                if (ModelState.IsValid)
                    return View("RequestPasswordStep2Success");
            }

            return View("RequestPasswordStep2");
        }
        #endregion
    }
}
