using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Account;
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
        private readonly UpdateApplicantService _updateApplicantService;
        private readonly UpdateApplicantFileService _updateApplicantFileService;

        public AccountController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService jobsService, UpdateApplicantService updateApplicantService, UpdateApplicantFileService updateApplicantFileService)
            : base(apiClient, settings)
        {
            _updateApplicantFileService = updateApplicantFileService;
            _updateApplicantService = updateApplicantService;
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
        public async Task<ActionResult> Login(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicantResponse applicant;
                try
                {
                    applicant = await new ApplicantRequest(model.Username, model.Password).LoadResult(ApiClient);
                }
                catch (NotFoundException)
                {
                    applicant = null;
                }

                if (applicant != null)
                {
                    FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);

                    if (model.JobId.HasValue)
                        return RedirectToAction("Index", "Application", new { job = model.JobId });
                    return RedirectToAction("Index", "Applicant");
                }

                ModelState.AddModelError("username", @"Error-InvalidUsernameOrPassword");
            }
            return View(model);
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(int? job, RegisterViewModel model)
        {
            var mandator = await GetMandator();
            model.Prepare(mandator, job.HasValue ? await GetJob(mandator, _jobsService, job.Value) : null);

            if (model.Cv != null && _updateApplicantFileService.CheckFile(model.Cv, "Cv") != UpdateApplicantFileService.CheckFileResult.Ok)
                ModelState.AddModelError("Cv", @"Error-Cv");
            if (model.Photo != null && _updateApplicantFileService.CheckFile(model.Photo, "Photo") != UpdateApplicantFileService.CheckFileResult.Ok)
                ModelState.AddModelError("Photo", @"Error-Photo");
            if (model.Documents != null)
            {
                var index = 0;
                foreach (var document in model.Documents)
                {
                    if (document != null && _updateApplicantFileService.CheckFile(document, model.DocumentTypes.ElementAt(index)) != UpdateApplicantFileService.CheckFileResult.Ok)
                    {
                        ModelState.AddModelError("Documents", @"Error-Documents");
                        break;
                    }
                    index++;
                }
            }

            if (ModelState.IsValid)
            {
                //check if the email is not in use already
                if (!mandator.PortalSettings.AllowDuplicateEmail)
                {
                    var applicantsWithThisEmail = await new ApplicantsRequest(model.Email).LoadResult(ApiClient);
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
                var applicant = await new ApplicantRequest(createParameter).LoadResult(ApiClient);

                //update the personal information
                applicant = await _updateApplicantService.UpdatePersonalInformation(ApiClient, applicant, model);

                //save the documents
                if (model.Cv != null)
                    await _updateApplicantFileService.AddFile(ApiClient, applicant, model.Cv, "Cv");
                if (model.Photo != null)
                    await _updateApplicantFileService.AddFile(ApiClient, applicant, model.Photo, "Photo");
                if (model.Documents != null)
                {
                    var index = 0;
                    foreach (var document in model.Documents)
                    {
                        if (document != null)
                            await _updateApplicantFileService.AddFile(ApiClient, applicant, document, model.DocumentTypes.ElementAt(index));
                        index++;
                    }
                }

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
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (model.NewPassword != model.NewPassword2)
                ModelState.AddModelError("NewPassword", @"Error-PasswordsDontMatch");

            if (ModelState.IsValid)
            {
                var requestParam = new ApplicantSetPasswordParameter(model.OldPassword, model.NewPassword, Request.Url);
                var result = await new SetPasswordRequest(GetApplicantId(), requestParam).LoadResult(ApiClient);
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
        public async Task<ActionResult> RequestPassword(string email, string code)
        {
            return await RequestPasswordStep2(email, code);
        }

        public ActionResult RequestPasswordStep1()
        {
            return View(new RequestPasswordStep1ViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestPasswordStep1(RequestPasswordStep1ViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await new RequestPasswordRequest(model.Email, Request.Url).LoadResult(ApiClient);
                }
                catch (NotFoundException)
                {
                    //do nothing here. When the request fails, we still want to show the success message (to avoid user enumeration security problem)
                }
                return View("RequestPasswordStep1Success");
            }
            return View(model);
        }

        public async Task<ActionResult> RequestPasswordStep2(string email, string code)
        {
            //check if the code really works, to display an early error message if not
            try
            {
                await new ConfirmRequestPasswordRequest(email, code).LoadResult(ApiClient);
            }
            catch
            {
                ModelState.AddModelError("", @"Error-InvalidCode");
            }

            return View("RequestPasswordStep2", new RequestPasswordStep2ViewModel { Code = code, Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestPasswordStep2(RequestPasswordStep2ViewModel model)
        {
            if (model.NewPassword != model.NewPassword2)
                ModelState.AddModelError("NewPassword", @"Error-PasswordsDontMatch");

            if (ModelState.IsValid)
            {
                var requestParam = new ApplicantSetPasswordAfterRequestParameter(model.Email, model.Code, model.NewPassword, Request.Url);
                var result = await new SetPasswordRequest(requestParam).LoadResult(ApiClient);
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("NewPassword", @"Error-" + error);

                if (ModelState.IsValid)
                    return View("RequestPasswordStep2Success");
            }

            return View(model);
        }
        #endregion
    }
}
