using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Utilities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    public class ControllerBase : Controller
    {
        protected ApiHttpClient ApiClient { get; set; }
        internal CustomSettings Settings { get; set; } //internal, because we need the settings in the ViewEngine

        protected ControllerBase(ApiHttpClient apiClient, CustomSettings customSettings)
        {
            ApiClient = apiClient;
            Settings = customSettings;
        }

        protected async Task<Mandator> GetMandator()
        {
            var mandator = await ApiClient.SendAndReadAsyncCached<Mandator>(new MandatorRequest(Request.Url));
            new CombinePortalAndCustomSettingsService().UpdatePortalSettingsWithCustomSettings(mandator.Settings, Settings);
            return mandator;
        }

        protected async Task<Applicant> GetApplicant()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            var applicant = await ApiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(GetApplicantId()));
            if (applicant == null || applicant.Id <= 0)
                return null;
            return applicant;
        }

        protected int GetApplicantId()
        {
            if (User.Identity.IsAuthenticated)
                return User.Identity.Name.GetInt();
            throw new ApplicationException("No applicant is logged in at the moment.");
        }

        public string TlT(string originalText)
        {
            var allTranslations = GetMandator().Result.Translations;
            var matchingEntry = allTranslations.FirstOrDefault(x => x.Texts.Any(y => y.Text.Is(originalText)));
            if (matchingEntry != null)
            {
                var currentCulture = CultureInfo.CurrentCulture.Name;

                //check for identical culture
                var matchingTranslation = matchingEntry.Texts.FirstOrDefault(x => x.Culture.Is(currentCulture));

                //check for similar culture
                matchingTranslation = matchingTranslation ?? matchingEntry.Texts.FirstOrDefault(x => x.Culture.Substring(0, 2).Is(currentCulture.Substring(0, 2)));

                //fallback to english
                matchingTranslation = matchingTranslation ?? matchingEntry.Texts.FirstOrDefault(x => x.Culture.Substring(0, 2).Is("en"));

                if (matchingTranslation != null)
                    return matchingTranslation.Text;
            }
            return originalText;
        }
    }
}
