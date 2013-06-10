using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Account;
using ePunkt.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace ePunkt.Portal.Controllers
{
    public class AccountController : ControllerBase
    {
        #region Constructor
        private readonly LoadJobsService _jobsService;

        public AccountController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService jobsService)
            : base(apiClient, settings)
        {
            _jobsService = jobsService;
        }
        #endregion

        #region Login / Logoff
        public ActionResult Login(int? job)
        {
            return View(new IndexViewModel { JobId = job });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password, int? job)
        {
            var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(username, password));
            if (applicant != null)
            {
                FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);

                if (job.HasValue)
                    return RedirectToAction("Index", "Application", new { job });
                return RedirectToAction("Index", "Applicant");
            }

            ModelState.AddModelError("username", @"Error-InvalidUsernameOrPassword");
            return View(new IndexViewModel { JobId = job });
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Jobs");
        }
        #endregion

        #region Register
        public async Task<ActionResult> Register(int? job)
        {
            if (User.Identity.IsAuthenticated && job.HasValue)
                return RedirectToAction("Index", "Application", new { job });
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Applicant");

            var mandator = await GetMandator();
            var model = new RegisterViewModel().Prepare(mandator, job.HasValue ? await GetJob(mandator, _jobsService, job.Value) : null);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(int? job, RegisterViewModel model)
        {
            var mandator = await GetMandator();
            model.Prepare(mandator, job.HasValue ? await GetJob(mandator, _jobsService, job.Value) : null);
            if (ModelState.IsValid)
            {
                //check if the email is not in use already
                if (!mandator.Settings.AllowDuplicateEmail)
                {
                    var applicantsWithThisEmail = await ApiClient.SendAndReadAsync<IEnumerable<Applicant>>(new ApplicantsRequest(model.Email));
                    if (applicantsWithThisEmail.Any())
                        return RedirectToAction("EmailAlreadyInUse", "Account", new { job, email = model.Email });
                }

                //create the applicant
                var createParameter = new ApplicantCreateParameter
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender
                };
                var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(createParameter));

                //set the remaining applicant data by updating it
                var updateParameter = new ApplicantUpdateParameter(applicant)
                    {
                        BirthDate = model.BirthDate,
                        City = model.City,
                        Country = model.Country,
                        Nationality = model.Nationality,
                        Phone = model.Phone,
                        Street = model.Street,
                        TitleBeforeName = model.TitleBeforeName,
                        TitleAfterName = model.TitleAfterName,
                        ZipCode = model.ZipCode
                    };
                applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(applicant.Id, updateParameter));

                //log the applicant in and redirect either to the applicant profile or the application page
                FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);
                if (job.HasValue)
                    return RedirectToAction("Index", "Application", new { job });
                return RedirectToAction("RegisterSuccess");
            }
            return View(model);
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        public ActionResult EmailAlreadyInUse(string email, int? job)
        {
            var model = new EmailAlreadyInUseViewModel
                {
                    Email = email,
                    JobId = job
                };
            return View(model);
        }
        #endregion

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
