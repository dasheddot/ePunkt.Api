using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System;
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

        protected async Task<MandatorResponse> GetMandator()
        {
            var mandator = await ApiClient.SendAndReadAsyncCached<MandatorResponse>(new MandatorRequest(Request.Url));
            new CombinePortalAndCustomSettingsService().UpdatePortalSettingsWithCustomSettings(mandator.PortalSettings, Settings);
            return mandator;
        }

        protected async Task<ApplicantResponse> GetApplicant()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            var applicant = await ApiClient.SendAndReadAsync<ApplicantResponse>(new ApplicantRequest(GetApplicantId()));
            if (applicant == null || applicant.Id <= 0)
                return null;
            return applicant;
        }

        protected async Task<JobResponse> GetJob(LoadJobsService jobsService, int jobId)
        {
            return await GetJob(await GetMandator(), jobsService, jobId);
        }

        protected async Task<JobResponse> GetJob(MandatorResponse mandatorResponse, LoadJobsService jobsService, int jobId)
        {
            var jobsResponse = await jobsService.LoadJobsForCurrentPortal(Request.Url, mandatorResponse);
            return jobsResponse.Jobs.FirstOrDefault(x => x.Id == jobId);
        }

        protected int GetApplicantId()
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name.IsInt() && User.Identity.Name.GetInt() > 0)
                return User.Identity.Name.GetInt();
            throw new ApplicationException("No applicant is logged in at the moment.");
        }

        public string TlT(string originalText)
        {
            return Translations.TlT(GetMandator().Result, originalText);
        }


    }
}
