using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Account;
using ePunkt.Portal.Models.ThirdPartyProfile;
using ePunkt.SocialConnector;
using ePunkt.SocialConnector.Linkedin;
using ePunkt.SocialConnector.Xing;
using ePunkt.Utilities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using IndexViewModel = ePunkt.Portal.Models.ThirdPartyProfile.IndexViewModel;

namespace ePunkt.Portal.Controllers
{
    public class ThirdPartyProfileController : ControllerBase
    {
        private readonly LinkThirdPartyProfileService _linkService;

        public ThirdPartyProfileController(ApiHttpClient apiClient, CustomSettings settings, LinkThirdPartyProfileService linkService)
            : base(apiClient, settings)
        {
            _linkService = linkService;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var applicant = await GetApplicant();

            var xingProfile = applicant.ThirdPartyProfiles.FirstOrDefault(x => x.ThirdParty.Is("Xing"));
            var linkedInProfile = applicant.ThirdPartyProfiles.FirstOrDefault(x => x.ThirdParty.Is("LinkedIn"));
            var model = new IndexViewModel
                {
                    IsLinkedInLinked = linkedInProfile != null,
                    LinkedInUrl = linkedInProfile != null ? linkedInProfile.Url : string.Empty,
                    IsXingLinked = xingProfile != null,
                    XingUrl = xingProfile != null ? xingProfile.Url : string.Empty
                };

            return View(model);
        }

        public async Task<ActionResult> Xing(int? job)
        {
            var mandator = await GetMandator();
            var tokenManager = new InMemoryTokenManager(mandator.PortalSettings.XingConsumerKey, mandator.PortalSettings.XingConsumerSecret);
            var xingConsumer = new XingConsumer(tokenManager);

            return await Link(xingConsumer, ThirdParty.Xing, job);
        }

        public async Task<ActionResult> LinkedIn(int? job)
        {
            var mandator = await GetMandator();
            var tokenManager = new InMemoryTokenManager(mandator.PortalSettings.LinkedinConsumerKey, mandator.PortalSettings.LinkedinConsumerSecret);
            var linkedinConsumer = new LinkedinConsumer(tokenManager);

            return await Link(linkedinConsumer, ThirdParty.LinkedIn, job);
        }

        [Authorize]
        public async Task<ActionResult> UnlinkXing()
        {
            await new UnlinkXingRequest(GetApplicantId()).LoadResult(ApiClient);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> UnlinkLinkedIn()
        {
            await new UnlinkLinkedInRequest(GetApplicantId()).LoadResult(ApiClient);
            return RedirectToAction("Index");
        }

        private async Task<ActionResult> Link(IConsumer consumer, ThirdParty thirdParty, int? job)
        {
            string accessToken;
            ActionResult result;

            try
            {
                result = consumer.ProcessAuthorization(Request, Url.Action("Index", "Applicant"), out accessToken);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Reason = ex.Message });
            }

            if (accessToken.HasValue())
            {
                var client = consumer.GetClient(accessToken);
                var profile = await client.Users.ForMe();

                var mandator = await GetMandator();
                var applicant = await GetApplicant();
                ActionResult onSuccessRedirectTo;
                if (applicant == null)
                {
                    applicant = await _linkService.LinkProfileToApplicant(ApiClient, mandator, profile, thirdParty);

                    //the applicant is null when the e-mail already exists for another applicant
                    if (applicant == null)
                        return RedirectToAction("EmailAlreadyInUse", "Account", new EmailAlreadyInUseViewModel { Email = profile.Email, JobId = job });

                    if (job.HasValue)
                        onSuccessRedirectTo = RedirectToAction("Index", "Application", new { job });
                    else if (applicant.DateOfCreation >= DateTime.Now.AddSeconds(-10)) //the applicant has just been created, redirect him to the "thank you for your registration" page
                        onSuccessRedirectTo = RedirectToAction("RegisterSuccess", "Account");
                    else
                        onSuccessRedirectTo = RedirectToAction("Index", "Applicant");
                }
                else
                {
                    applicant = await _linkService.LinkProfileToApplicant(ApiClient, mandator, applicant, profile, thirdParty);
                    onSuccessRedirectTo = RedirectToAction("Index", "ThirdPartyProfile");
                }

                return SetAuthCookieOrRedirectToErrorPage(applicant, onSuccessRedirectTo);
            }

            return result;
        }

        private ActionResult SetAuthCookieOrRedirectToErrorPage(ApplicantResponse applicantResponse, ActionResult onSuccessRedirectTo)
        {
            if (applicantResponse != null)
            {
                FormsAuthentication.SetAuthCookie(applicantResponse.Id.ToString(CultureInfo.InvariantCulture), false);
                return onSuccessRedirectTo;
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
