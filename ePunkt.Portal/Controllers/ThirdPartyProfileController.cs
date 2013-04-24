using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
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

        public ActionResult EmailAlreadyInUse(string email)
        {
            return View((object)email);
        }

        public async Task<ActionResult> Xing()
        {
            var mandator = await GetMandator();
            var tokenManager = new InMemoryTokenManager(mandator.Settings.XingConsumerKey, mandator.Settings.XingConsumerSecret);
            var xingConsumer = new XingConsumer(tokenManager);

            return await Link(xingConsumer, ThirdParty.Xing);
        }

        public async Task<ActionResult> LinkedIn()
        {
            var mandator = await GetMandator();
            var tokenManager = new InMemoryTokenManager(mandator.Settings.LinkedinConsumerKey, mandator.Settings.LinkedinConsumerSecret);
            var linkedinConsumer = new LinkedinConsumer(tokenManager);

            return await Link(linkedinConsumer, ThirdParty.LinkedIn);
        }

        [Authorize]
        public async Task<ActionResult> UnlinkXing()
        {
            await ApiClient.SendAndReadAsync<Applicant>(new UnlinkXingRequest(GetApplicantId()));
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> UnlinkLinkedIn()
        {
            await ApiClient.SendAndReadAsync<Applicant>(new UnlinkLinkedInRequest(GetApplicantId()));
            return RedirectToAction("Index");
        }

        private async Task<ActionResult> Link(IConsumer consumer, ThirdParty thirdParty)
        {
            string accessToken;
            ActionResult result;

            try
            {
                result = consumer.ProcessAuthorization(Request, Url.Action("Index", "Applicant"), out accessToken);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel {Reason = ex.Message});
            }

            if (accessToken.HasValue())
            {
                var xingClient = consumer.GetClient(accessToken);
                var profile = await xingClient.Users.ForMe();

                var mandator = await GetMandator();
                var applicant = await GetApplicant();
                ActionResult onSuccessRedirectTo;
                if (applicant == null)
                {
                    applicant = await _linkService.LinkProfileToApplicant(ApiClient, mandator, profile, thirdParty);

                    //the applicant is null when the e-mail already exists for another applicant
                    if (applicant == null)
                        return RedirectToAction("EmailAlreadyInUse", new {email = profile.Email});

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

        private ActionResult SetAuthCookieOrRedirectToErrorPage(Applicant applicant, ActionResult onSuccessRedirectTo)
        {
            if (applicant != null)
            {
                FormsAuthentication.SetAuthCookie(applicant.Id.ToString(CultureInfo.InvariantCulture), false);
                return onSuccessRedirectTo;
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
